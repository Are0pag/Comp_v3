namespace WPF.Services.Validation;

public abstract class ValidatorBase<T> : IValidator<T>
{
    private readonly Dictionary<string, IValidationRule<T>> _rules = new();

    protected ValidatorBase() {
        SetRules();
    }
    protected abstract void SetRules();

    /// Валидация только одного свойства
    public async Task<ValidationResult> ValidatePropertyAsync(T value, string propertyName) {
        var result = new ValidationResult { IsValid = true };
        var propertyRules = _rules.Values.Where(r => r.PropertyName == propertyName);

        foreach (var rule in propertyRules) {
            var ruleResult = await rule.ValidateAsync(value);

            if (!ruleResult.IsValid) {
                result.IsValid = false;
                result.Errors.AddRange(ruleResult.Errors);
            }
        }

        return result;
    }

    public async Task<ValidationResult> ValidateAsync(T value) {
        var result = new ValidationResult { IsValid = true };

        foreach (var rule in _rules.Values) {
            var ruleResult = await rule.ValidateAsync(value);

            if (!ruleResult.IsValid) {
                result.IsValid = false;
                result.Errors.AddRange(ruleResult.Errors);
            }

            result.Warnings.AddRange(ruleResult.Warnings);
        }

        return result;
    }

    public void AddRule(IValidationRule<T> rule) {
        _rules[rule.RuleName] = rule;
    }

    public void RemoveRule(string ruleName) {
        _rules.Remove(ruleName);
    }

    static public ValidationRuleBuilder<T> CreateRules() {
        return new ValidationRuleBuilder<T>();
    }
}
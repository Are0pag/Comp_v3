namespace WPF.Services.Validation;

public abstract class ValidatorBase<T> : IValidator<T>
{
    private readonly List<IValidationRule<T>> _rules = new();

    protected ValidatorBase() {
        SetRules();
    }
    protected abstract void SetRules();

    /// Валидация только одного свойства
    public async Task<ValidationResult> ValidatePropertyAsync(T value, string propertyName) {
        var result = new ValidationResult { IsValid = true };
        var propertyRules = _rules.Where(r => r.PropertyName == propertyName);

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

        foreach (var rule in _rules) {
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
        _rules.Add(rule);
    }

    public void RemoveRule(string ruleName) {
        if (_rules.FirstOrDefault(r => r.RuleName == ruleName) is not { } rule) {
            throw new InvalidOperationException($"Rule {ruleName} not found");
        }
        _rules.Remove(rule);
    }

    static public ValidationRuleBuilder<T> CreateRules() {
        return new ValidationRuleBuilder<T>();
    }
}
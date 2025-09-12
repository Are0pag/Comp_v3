namespace WPF.Services.Validation;

public class Validator<T> : IValidator<T>
{
    private readonly Dictionary<string, IValidationRule<T>> _rules = new();

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
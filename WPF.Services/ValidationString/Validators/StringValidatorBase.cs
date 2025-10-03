using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public abstract class StringValidatorBase : IStringValidator
{
    private readonly Dictionary<string, IStringValidationRule> _rules = new();

    protected StringValidatorBase() {
        SetRules();
    }

    protected abstract void SetRules();

    public async Task<ValidationResult> ValidateAsync(string value) {
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

    public async Task<ValidationResult> ValidateAsync(string value, string propertyName) {
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

    public void AddRule(IStringValidationRule rule) {
        _rules[rule.RuleName] = rule;
    }

    public void RemoveRule(string ruleName) {
        _rules.Remove(ruleName);
    }

    public static StringValidationRuleBuilder CreateRules() {
        return new StringValidationRuleBuilder();
    }
}
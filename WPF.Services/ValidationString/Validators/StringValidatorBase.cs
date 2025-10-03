using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public abstract class StringValidatorBase : IStringValidator
{
    private readonly Dictionary<string, IStringValidationRule> _rules = new();

    protected StringValidatorBase() {
        SetRules();
    }

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

    public void AddRule(IStringValidationRule rule) {
        _rules[rule.RuleName] = rule;
    }

    public void RemoveRule(string ruleName) {
        _rules.Remove(ruleName);
    }

    protected abstract void SetRules();

    public static StringValidationRuleBuilder CreateRules() {
        return new StringValidationRuleBuilder();
    }
}
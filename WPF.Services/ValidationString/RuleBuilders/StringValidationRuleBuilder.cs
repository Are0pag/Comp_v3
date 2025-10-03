using System.Text.RegularExpressions;
using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class StringValidationRuleBuilder
{
    private readonly List<IStringValidationRule> _rules = new();

    public StringValidationRuleBuilder Required(string errorMessage = "Field is required") {
        _rules.Add(new RequiredStringRule(errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Length(int min, int max, string errorMessage = "Invalid length") {
        _rules.Add(new LengthStringRule(min, max, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Custom(Func<string, bool> validationFunc,
                                              string ruleName = "",
                                              string errorMessage = "",
                                              ValidationSeverity severity = ValidationSeverity.Error) {
        _rules.Add(new CustomStringValidationRule(ruleName, errorMessage, severity, validationFunc));
        return this;
    }

    public StringValidationRuleBuilder Regex(string pattern, string errorMessage = "Invalid format") {
        _rules.Add(new RegexStringRule(pattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Regex(string pattern, RegexOptions options,
                                             string errorMessage = "Invalid format") {
        _rules.Add(new AdvancedRegexStringRule(pattern, options, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Email(string errorMessage = "Invalid email format") {
        var emailPattern = RegexPatternBuilder.Create()
                                              .StartsWith()
                                              .Email()
                                              .EndsWith()
                                              .Build();

        _rules.Add(new RegexStringRule(emailPattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Phone(string errorMessage = "Invalid phone format") {
        var phonePattern = RegexPatternBuilder.Create()
                                              .StartsWith()
                                              .Phone()
                                              .EndsWith()
                                              .Build();

        _rules.Add(new RegexStringRule(phonePattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder DigitsOnly(string errorMessage = "Only digits are allowed") {
        var pattern = RegexPatternBuilder.Create()
                                         .StartsWith()
                                         .Digits()
                                         .EndsWith()
                                         .Build();

        _rules.Add(new RegexStringRule(pattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder LettersOnly(string errorMessage = "Only letters are allowed") {
        var pattern = RegexPatternBuilder.Create()
                                         .StartsWith()
                                         .Letters()
                                         .EndsWith()
                                         .Build();

        _rules.Add(new RegexStringRule(pattern, errorMessage));
        return this;
    }

    public IEnumerable<IStringValidationRule> Build() {
        return _rules;
    }
}
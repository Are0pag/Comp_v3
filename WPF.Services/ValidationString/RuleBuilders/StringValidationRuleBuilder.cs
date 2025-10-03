using System.Text.RegularExpressions;
using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class StringValidationRuleBuilder
{
    private readonly List<IStringValidationRule> _rules = new();
    private string _currentPropertyName = "Value";

    public StringValidationRuleBuilder ForProperty(string propertyName) {
        _currentPropertyName = propertyName;
        return this;
    }

    public StringValidationRuleBuilder Required(string errorMessage = "Field is required") {
        _rules.Add(new RequiredStringRule(_currentPropertyName, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Length(int min, int max, string errorMessage = "Invalid length") {
        _rules.Add(new LengthStringRule(_currentPropertyName, min, max, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder MinLength(int min, string errorMessage = "Value is too short") {
        _rules.Add(new MinLengthStringRule(_currentPropertyName, min, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder MaxLength(int max, string errorMessage = "Value is too long") {
        _rules.Add(new MaxLengthStringRule(_currentPropertyName, max, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Custom(Func<string, bool> validationFunc,
                                              string ruleName = "",
                                              string errorMessage = "",
                                              ValidationSeverity severity = ValidationSeverity.Error) {
        _rules.Add(new CustomStringValidationRule(_currentPropertyName, ruleName, errorMessage, severity,
                                                  validationFunc));
        return this;
    }

    public StringValidationRuleBuilder Regex(string pattern, string errorMessage = "Invalid format") {
        _rules.Add(new RegexStringRule(_currentPropertyName, pattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Regex(string pattern, RegexOptions options,
                                             string errorMessage = "Invalid format") {
        _rules.Add(new AdvancedRegexStringRule(_currentPropertyName, pattern, options, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Email(string errorMessage = "Invalid email format") {
        var emailPattern = RegexPatternBuilder.Create()
                                              .StartsWith()
                                              .Email()
                                              .EndsWith()
                                              .Build();

        _rules.Add(new RegexStringRule(_currentPropertyName, emailPattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder Phone(string errorMessage = "Invalid phone format") {
        var phonePattern = RegexPatternBuilder.Create()
                                              .StartsWith()
                                              .Phone()
                                              .EndsWith()
                                              .Build();

        _rules.Add(new RegexStringRule(_currentPropertyName, phonePattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder DigitsOnly(string errorMessage = "Only digits are allowed") {
        var pattern = RegexPatternBuilder.Create()
                                         .StartsWith()
                                         .Digits()
                                         .EndsWith()
                                         .Build();

        _rules.Add(new RegexStringRule(_currentPropertyName, pattern, errorMessage));
        return this;
    }

    public StringValidationRuleBuilder LettersOnly(string errorMessage = "Only letters are allowed") {
        var pattern = RegexPatternBuilder.Create()
                                         .StartsWith()
                                         .Letters()
                                         .EndsWith()
                                         .Build();

        _rules.Add(new RegexStringRule(_currentPropertyName, pattern, errorMessage));
        return this;
    }

    public IEnumerable<IStringValidationRule> Build() {
        return _rules;
    }
}
using System.Text.RegularExpressions;
using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public interface IStringValidator
{
    Task<ValidationResult> ValidateAsync(string value);
    Task<ValidationResult> ValidateAsync(string value, string propertyName);
    void AddRule(IStringValidationRule rule);
    void RemoveRule(string ruleName);
}


public class MinLengthStringRule : BaseStringValidationRule
{
    private readonly int _minLength;

    public MinLengthStringRule(string propertyName, int minLength, string errorMessage = "Value is too short")
        : base(propertyName, "MinLength", errorMessage) {
        _minLength = minLength;
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        if (string.IsNullOrEmpty(value))
            return CreateErrorResult("Value cannot be null or empty");

        var isValid = value.Length >= _minLength;
        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}

public class MaxLengthStringRule : BaseStringValidationRule
{
    private readonly int _maxLength;

    public MaxLengthStringRule(string propertyName, int maxLength, string errorMessage = "Value is too long")
        : base(propertyName, "MaxLength", errorMessage) {
        _maxLength = maxLength;
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        if (value == null)
            return CreateSuccessResult(); // null считается валидным для MaxLength

        var isValid = value.Length <= _maxLength;
        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}

public class AdvancedRegexStringRule : BaseStringValidationRule
{
    private readonly Regex _regex;

    public AdvancedRegexStringRule(string propertyName, string pattern, RegexOptions options,
                                   string errorMessage = "Invalid format")
        : base(propertyName, "AdvancedRegex", errorMessage) {
        _regex = new Regex(pattern, options);
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        if (string.IsNullOrEmpty(value))
            return CreateErrorResult("Value cannot be null or empty for regex validation");

        var isValid = _regex.IsMatch(value);
        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}
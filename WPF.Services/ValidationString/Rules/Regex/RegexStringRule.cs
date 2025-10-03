using System.Text.RegularExpressions;
using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class RegexStringRule : BaseStringValidationRule
{
    private readonly Regex _regex;

    public RegexStringRule(string pattern, string propertyName = "", string errorMessage = "Invalid format")
        : base(propertyName, "Regex", errorMessage) {
        _regex = new Regex(pattern);
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        if (string.IsNullOrEmpty(value))
            return CreateErrorResult("Value cannot be null or empty for regex validation");

        var isValid = _regex.IsMatch(value);
        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}
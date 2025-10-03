using System.Text.RegularExpressions;
using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class AdvancedRegexStringRule : BaseStringValidationRule
{
    private readonly Regex _regex;

    public AdvancedRegexStringRule(string pattern, RegexOptions options, string errorMessage = "Invalid format")
        : base("AdvancedRegex", errorMessage) {
        _regex = new Regex(pattern, options);
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        if (string.IsNullOrEmpty(value))
            return CreateErrorResult("Value cannot be null or empty for regex validation");

        var isValid = _regex.IsMatch(value);
        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}
using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class RequiredStringRule : BaseStringValidationRule
{
    public RequiredStringRule(string errorMessage = "Field is required")
        : base("Required", errorMessage) {
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        var isValid = !string.IsNullOrWhiteSpace(value);
        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}
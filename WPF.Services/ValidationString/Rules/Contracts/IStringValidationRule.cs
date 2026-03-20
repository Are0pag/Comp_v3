using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public interface IStringValidationRule
{
    string RuleName { get; }
    string ErrorMessage { get; }
    ValidationSeverity Severity { get; }
    Task<ValidationResult> ValidateAsync(string value);
}
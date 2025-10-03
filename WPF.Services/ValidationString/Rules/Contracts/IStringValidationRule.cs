using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public interface IStringValidationRule
{
    string RuleName { get; }
    string ErrorMessage { get; }
    ValidationSeverity Severity { get; }
    string PropertyName { get; }
    Task<ValidationResult> ValidateAsync(string value);
}
namespace WPF.Services.Validation;

public interface IValidationRule<T>
{
    string RuleName { get; }
    string PropertyName { get; }
    string ErrorMessage { get; }
    ValidationSeverity Severity { get; }
    Task<ValidationResult> ValidateAsync(T item);
}
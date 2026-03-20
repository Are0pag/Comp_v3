using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public abstract class BaseStringValidationRule : IStringValidationRule
{
    protected BaseStringValidationRule(string ruleName, string errorMessage) {
        RuleName = ruleName;
        ErrorMessage = errorMessage;
    }

    public string RuleName { get; protected set; }
    public string ErrorMessage { get; protected set; }
    public ValidationSeverity Severity { get; protected set; } = ValidationSeverity.Error;

    public abstract Task<ValidationResult> ValidateAsync(string value);

    protected Task<ValidationResult> CreateErrorResult(string message = null) {
        var errorMessage = message ?? ErrorMessage;

        return Task.FromResult(new ValidationResult {
            IsValid = false,
            Errors = new List<ValidationError> {
                new() {
                    PropertyName = "Value", // Фиксированное имя, так как валидируем только значение
                    ErrorMessage = errorMessage,
                    Severity = Severity,
                    RuleName = RuleName
                }
            }
        });
    }

    protected Task<ValidationResult> CreateSuccessResult() {
        return Task.FromResult(new ValidationResult {
            IsValid = true,
            Errors = new List<ValidationError>()
        });
    }
}
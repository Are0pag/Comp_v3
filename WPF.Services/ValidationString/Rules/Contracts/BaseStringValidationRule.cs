using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public abstract class BaseStringValidationRule : IStringValidationRule
{
    public string RuleName { get; protected set; }
    public string ErrorMessage { get; protected set; }
    public ValidationSeverity Severity { get; protected set; } = ValidationSeverity.Error;
    public string PropertyName { get; protected set; }

    protected BaseStringValidationRule(string propertyName, string ruleName, string errorMessage) {
        PropertyName = propertyName;
        RuleName = ruleName;
        ErrorMessage = errorMessage;
    }

    public abstract Task<ValidationResult> ValidateAsync(string value);

    protected Task<ValidationResult> CreateErrorResult(string message = null) {
        var errorMessage = message ?? ErrorMessage;

        return Task.FromResult(new ValidationResult {
            IsValid = false,
            Errors = new List<ValidationError> {
                new() {
                    PropertyName = PropertyName,
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
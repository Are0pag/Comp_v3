using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class CustomStringValidationRule : BaseStringValidationRule
{
    private readonly Func<string, bool> _validationFunc;

    public CustomStringValidationRule(string propertyName, string ruleName, string errorMessage,
                                      ValidationSeverity severity, Func<string, bool> validationFunc)
        : base(propertyName, ruleName, errorMessage) {
        Severity = severity;
        _validationFunc = validationFunc;
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        var isValid = _validationFunc(value);
        return Task.FromResult(new ValidationResult {
            IsValid = isValid,
            Errors = isValid
                ? new List<ValidationError>()
                : new List<ValidationError> {
                    new() {
                        PropertyName = PropertyName,
                        ErrorMessage = ErrorMessage,
                        Severity = Severity,
                        RuleName = RuleName
                    }
                }
        });
    }
}
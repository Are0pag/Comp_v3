namespace WPF.Services.Validation;

public class CustomValidationRule<T> : BaseValidationRule<T>
{
    private readonly Func<T, bool> _validationFunc;

    public CustomValidationRule(string propertyName, string ruleName, string errorMessage, 
                                ValidationSeverity severity, Func<T, bool> validationFunc)
        : base(propertyName, ruleName, errorMessage)
    {
        Severity = severity;
        _validationFunc = validationFunc;
    }

    public override Task<ValidationResult> ValidateAsync(T item)
    {
        var isValid = _validationFunc(item);
        return Task.FromResult(new ValidationResult
        {
            IsValid = isValid,
            Errors = isValid ? new List<ValidationError>() : new List<ValidationError>
            {
                new ValidationError
                {
                    PropertyName = PropertyName,
                    ErrorMessage = ErrorMessage,
                    Severity = Severity,
                    RuleName = RuleName
                }
            }
        });
    }
}
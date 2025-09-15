namespace WPF.Services.Validation;

public class AsyncValidationRule<T> : BaseValidationRule<T>
{
    private readonly Func<T, Task<bool>> _asyncValidationFunc;

    public AsyncValidationRule(string propertyName, string ruleName, string errorMessage,
                               ValidationSeverity severity, Func<T, Task<bool>> asyncValidationFunc)
        : base(propertyName, ruleName, errorMessage)
    {
        Severity = severity;
        _asyncValidationFunc = asyncValidationFunc;
    }

    public override async Task<ValidationResult> ValidateAsync(T item)
    {
        var isValid = await _asyncValidationFunc(item);
        return new ValidationResult
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
        };
    }
}
namespace WPF.Services.Validation;

public abstract class BaseValidationRule<T> : IValidationRule<T>
{
    public string RuleName { get; protected set; }
    public string ErrorMessage { get; protected set; }
    public ValidationSeverity Severity { get; protected set; } = ValidationSeverity.Error;
    public string PropertyName { get; protected set; }

    protected BaseValidationRule(string propertyName, string ruleName, string errorMessage)
    {
        PropertyName = propertyName;
        RuleName = ruleName;
        ErrorMessage = errorMessage;
    }

    public abstract Task<ValidationResult> ValidateAsync(T item);
}
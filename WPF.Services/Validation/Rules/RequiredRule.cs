namespace WPF.Services.Validation;

public class RequiredRule<T> : BaseValidationRule<T>
{
    public RequiredRule(string propertyName, string errorMessage = "Field is required")
        : base(propertyName, "Required", errorMessage) { }

    public override Task<ValidationResult> ValidateAsync(T value)
    {
        var isValid = value != null && !string.IsNullOrWhiteSpace(value.ToString());
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
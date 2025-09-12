namespace WPF.Services.Validation;

public class LengthRule<T> : BaseValidationRule<T>
{
    private readonly int _minLength;
    private readonly int _maxLength;

    public LengthRule(string propertyName, int minLength, int maxLength, 
                      string errorMessage = "Invalid length")
        : base(propertyName, "Length", errorMessage)
    {
        _minLength = minLength;
        _maxLength = maxLength;
    }

    public override Task<ValidationResult> ValidateAsync(T value)
    {
        var str = value?.ToString() ?? string.Empty;
        var isValid = str.Length >= _minLength && str.Length <= _maxLength;
        
        return Task.FromResult(new ValidationResult
        {
            IsValid = isValid,
            Errors = isValid ? new List<ValidationError>() : new List<ValidationError>
            {
                new ValidationError
                {
                    PropertyName = PropertyName,
                    ErrorMessage = $"{ErrorMessage} (min: {_minLength}, max: {_maxLength})",
                    Severity = Severity,
                    RuleName = RuleName
                }
            }
        });
    }
}
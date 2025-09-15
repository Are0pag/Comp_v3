namespace WPF.Services.Validation;

public class RegexRule<T> : BaseValidationRule<T>
{
    private readonly System.Text.RegularExpressions.Regex _regex;

    public RegexRule(string propertyName, string pattern, string errorMessage = "Invalid format")
        : base(propertyName, "Regex", errorMessage)
    {
        _regex = new System.Text.RegularExpressions.Regex(pattern);
    }

    public override Task<ValidationResult> ValidateAsync(T item)
    {
        var str = item?.ToString() ?? string.Empty;
        var isValid = _regex.IsMatch(str);
        
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
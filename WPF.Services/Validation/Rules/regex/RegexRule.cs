using System.Text.RegularExpressions;

namespace WPF.Services.Validation;

public class RegexRule<T> : BaseValidationRule<T>
{
    private readonly Regex _regex;

    public RegexRule(string propertyName, string pattern, string errorMessage = "Invalid format")
        : base(propertyName, "Regex", errorMessage)
    {
        _regex = new Regex(pattern);
    }

    public override Task<ValidationResult> ValidateAsync(T item)
    {
        if (item == null)
            return CreateErrorResult("Item is null");

        if (!TryGetPropertyValue(item, out var value))
            return CreateErrorResult($"Property '{PropertyName}' not found");

        // Для null значений считаем невалидным
        if (value == null)
            return CreateErrorResult("Value cannot be null for regex validation");

        var str = value.ToString();
        var isValid = _regex.IsMatch(str);
        
        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}
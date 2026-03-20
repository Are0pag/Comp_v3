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

    public override Task<ValidationResult> ValidateAsync(T item)
    {
        if (item == null)
            return CreateErrorResult("Item is null");

        if (!TryGetPropertyValue(item, out var value))
            return CreateErrorResult($"Property '{PropertyName}' not found");

        // Для null значений считаем невалидным (можно изменить логику если нужно)
        if (value == null)
            return CreateErrorResult("Value cannot be null for length validation");

        var str = value.ToString();
        var isValid = str.Length >= _minLength && str.Length <= _maxLength;
        
        if (isValid)
            return CreateSuccessResult();
        
        var detailedMessage = $"{ErrorMessage} (current: {str.Length}, required: {_minLength}-{_maxLength})";
        return CreateErrorResult(detailedMessage);
    }
}
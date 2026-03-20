using System.Collections;

namespace WPF.Services.Validation;

public class RequiredRule<T> : BaseValidationRule<T>
{
    public RequiredRule(string propertyName, string errorMessage = "Field is required")
        : base(propertyName, "Required", errorMessage) 
    {
    }

    public override Task<ValidationResult> ValidateAsync(T item)
    {
        if (item == null)
            return CreateErrorResult("Item is null");

        if (!TryGetPropertyValue(item, out var value))
            return CreateErrorResult($"Property '{PropertyName}' not found");

        return IsValueValid(value) 
            ? CreateSuccessResult() 
            : CreateErrorResult();
    }

    private bool IsValueValid(object value)
    {
        return value switch
        {
            null => false,
            // Для строк проверяем на пустоту
            string stringValue => !string.IsNullOrWhiteSpace(stringValue),
            // Для коллекций проверяем наличие элементов
            IEnumerable enumerable when value is not string => enumerable.Cast<object>().Any(),
            _ => true // Для остальных типов считаем валидным, если не null
        };
    }
}
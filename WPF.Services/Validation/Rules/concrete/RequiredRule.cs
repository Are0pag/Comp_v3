using System.Collections;

namespace WPF.Services.Validation;

public class RequiredRule<T> : BaseValidationRule<T>
{
    public RequiredRule(string propertyName, string errorMessage = "Field is required")
        : base(propertyName, "Required", errorMessage) {
    }

    public override Task<ValidationResult> ValidateAsync(T item) {
        if (item is null)
            return CreateResult(false, "Item is null");

        return typeof(T).GetProperty(PropertyName) is not { } propertyInfo
            ? CreateResult(false, $"Property '{PropertyName}' not found")
            : CreateResult(IsValueValid(propertyInfo.GetValue(item)));
    }

    private bool IsValueValid(object? value) {
        return value switch {
            null => false,
            // Для строк проверяем на пустоту
            string stringValue => !string.IsNullOrWhiteSpace(stringValue),
            // Для коллекций проверяем наличие элементов
            IEnumerable enumerable when value is not string => enumerable.Cast<object>().Any(),
            _ => true // Для остальных типов считаем валидным, если не null
        };
    }

    private Task<ValidationResult> CreateResult(bool isValid, string message = "") {
        return Task.FromResult(new ValidationResult {
            IsValid = isValid,
            Errors = new List<ValidationError> {
                new() {
                    PropertyName = PropertyName,
                    ErrorMessage = message,
                    Severity = Severity,
                    RuleName = RuleName
                }
            }
        });
    }
}
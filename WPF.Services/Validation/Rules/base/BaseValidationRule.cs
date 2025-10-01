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

    /// <summary>
    /// Получает значение свойства из объекта
    /// </summary>
    protected object GetPropertyValue(T item)
    {
        if (item == null) 
            throw new ArgumentNullException(nameof(item), "Validation item cannot be null");
        
        var propertyInfo = typeof(T).GetProperty(PropertyName);
        if (propertyInfo == null)
            throw new InvalidOperationException($"Property '{PropertyName}' not found on type {typeof(T).Name}");
            
        return propertyInfo.GetValue(item);
    }

    /// <summary>
    /// Безопасно пытается получить значение свойства
    /// </summary>
    protected bool TryGetPropertyValue(T item, out object value)
    {
        value = null;
        
        if (item == null) 
            return false;
        
        var propertyInfo = typeof(T).GetProperty(PropertyName);
        if (propertyInfo == null) 
            return false;
            
        value = propertyInfo.GetValue(item);
        return true;
    }

    /// <summary>
    /// Создает результат валидации с ошибкой
    /// </summary>
    protected Task<ValidationResult> CreateErrorResult(string message = null)
    {
        var errorMessage = message ?? ErrorMessage;
        
        return Task.FromResult(new ValidationResult {
            IsValid = false,
            Errors = new List<ValidationError> {
                new() {
                    PropertyName = PropertyName,
                    ErrorMessage = errorMessage,
                    Severity = Severity,
                    RuleName = RuleName
                }
            }
        });
    }

    /// <summary>
    /// Создает успешный результат валидации
    /// </summary>
    protected Task<ValidationResult> CreateSuccessResult()
    {
        return Task.FromResult(new ValidationResult {
            IsValid = true,
            Errors = new List<ValidationError>()
        });
    }
}
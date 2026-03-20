namespace WPF.Services.Validation;

public interface IValidator<T>
{
    Task<ValidationResult> ValidateAsync(T value);
    void AddRule(IValidationRule<T> rule);
    void RemoveRule(string ruleName);
}
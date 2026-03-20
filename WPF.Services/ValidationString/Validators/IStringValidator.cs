using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public interface IStringValidator
{
    Task<ValidationResult> ValidateAsync(string value);
    void AddRule(IStringValidationRule rule);
    void RemoveRule(string ruleName);
}
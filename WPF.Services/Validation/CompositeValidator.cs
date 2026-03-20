/*namespace WPF.Services.Validation;

public class CompositeValidator<T> : IValidator<T>
{
    private readonly List<IValidator<T>> _validators = new();

    public void AddValidator(IValidator<T> validator) {
        _validators.Add(validator);
    }

    public void RemoveValidator(IValidator<T> validator) {
        _validators.Remove(validator);
    }

    public async Task<ValidationResult> ValidateAsync(T value) {
        var result = new ValidationResult { IsValid = true };
        var tasks = _validators.Select(v => v.ValidateAsync(value));

        var results = await Task.WhenAll(tasks);

        foreach (var validationResult in results) {
            if (!validationResult.IsValid)
                result.IsValid = false;

            result.Errors.AddRange(validationResult.Errors);
            result.Warnings.AddRange(validationResult.Warnings);
        }

        return result;
    }

    // Реализуем методы интерфейса IValidator<T>
    public void AddRule(IValidationRule<T> rule) {
        // Для композитного валидатора добавляем правило в первый дочерний валидатор
        if (_validators.Count > 0) {
            _validators[0].AddRule(rule);
        }
        else {
            // Или создаем новый простой валидатор
            var validator = new ValidatorBase<T>();
            validator.AddRule(rule);
            _validators.Add(validator);
        }
    }

    public void RemoveRule(string ruleName) {
        foreach (var validator in _validators) validator.RemoveRule(ruleName);
    }
}*/
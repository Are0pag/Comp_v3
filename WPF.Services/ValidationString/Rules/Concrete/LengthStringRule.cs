using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class LengthStringRule : BaseStringValidationRule
{
    private readonly int _minLength;
    private readonly int _maxLength;

    public LengthStringRule(string propertyName, int minLength, int maxLength, string errorMessage = "Invalid length")
        : base(propertyName, "Length", errorMessage) {
        _minLength = minLength;
        _maxLength = maxLength;
    }

    public override Task<ValidationResult> ValidateAsync(string value) {
        if (string.IsNullOrEmpty(value))
            return CreateErrorResult("Value cannot be null or empty");

        var length = value.Length;
        var isValid = length >= _minLength && length <= _maxLength;

        return isValid ? CreateSuccessResult() : CreateErrorResult();
    }
}
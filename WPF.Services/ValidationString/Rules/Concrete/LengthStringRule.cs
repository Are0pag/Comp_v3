using WPF.Services.Validation;

namespace WPF.Services.ValidationString;

public class LengthStringRule : BaseStringValidationRule
{
    private readonly int _maxLength;
    private readonly int _minLength;

    public LengthStringRule(int minLength, int maxLength, string errorMessage = "Invalid length")
        : base("Length", errorMessage) {
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
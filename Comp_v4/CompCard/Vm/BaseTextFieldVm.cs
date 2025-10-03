using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public abstract class BaseTextFieldVm : BaseFieldVm
{
    protected readonly StringValidatorBase _validator;
    
    protected BaseTextFieldVm(StringValidatorBase validator) {
        _validator = validator;
    }
    
    public bool IsValid() {
        return _validator.ValidateAsync(_value).Result is { IsValid: true };
    }
}
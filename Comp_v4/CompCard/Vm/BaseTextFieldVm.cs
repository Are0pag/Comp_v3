using Comp_v4.CompCard.Events;
using Comp.ModelData.Comp;
using Utils.EventBus;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public abstract class BaseTextFieldVm : BaseFieldVm, ICompCardLoadedHandler
{
    protected readonly StringValidatorBase _validator;
    
    protected BaseTextFieldVm(StringValidatorBase validator) {
        _validator = validator;
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }
    
    public bool IsValid() {
        return _validator.ValidateAsync(_value).Result is { IsValid: true };
    }

    public virtual void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public abstract void OnCompCardLoaded(Component component);
}
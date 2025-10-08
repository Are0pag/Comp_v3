using Utils.EventBus;
using Utils.WPF.Buttons;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public abstract class BaseRequiredTextFieldVm : BaseTextFieldVm
{
    protected BaseRequiredTextFieldVm(StringValidatorBase validator) : base(validator) {
    }

    public override string Value {
        get => _value;
        set {
            if (value == _value) return;
            _value = value;
            OnPropertyChanged();
            EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
        }
    }
}
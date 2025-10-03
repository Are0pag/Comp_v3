using CommunityToolkit.Mvvm.ComponentModel;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public abstract class BaseVmForFieldWithButton : /*ObservableObject*/ BaseFieldVm
{
    protected readonly Action _openWindow;
    /*protected string _value;
    protected string _label;*/

    /*protected BaseVmForFieldWithButton(Action openWindow) {
        _openWindow = openWindow;
    }*/

    /*public string Value {
        get => _value;
        set {
            if (value == _value) return;
            _value = value;
            OnPropertyChanged();
        }
    }

    public string Label {
        get => _label;
        set {
            if (value == _label) return;
            _label = value;
            OnPropertyChanged();
        }
    }*/

    protected BaseVmForFieldWithButton(StringValidatorBase validator, Action openWindow) : base(validator) {
        _openWindow = openWindow;
    }

    protected abstract void OpenNestedWindow();
}
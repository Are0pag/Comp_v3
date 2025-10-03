using CommunityToolkit.Mvvm.ComponentModel;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.CompCard.Vm;

public abstract class BaseFieldVm : ObservableObject
{
    
    protected string _value;
    protected string _label;

    public string Value {
        get => _value;
        set {
            if (value == _value) return;
            _value = value;
            OnPropertyChanged();
            EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
        }
    }

    public string Label {
        get => _label;
        set {
            if (value == _label) return;
            _label = value;
            OnPropertyChanged();
        }
    }
}
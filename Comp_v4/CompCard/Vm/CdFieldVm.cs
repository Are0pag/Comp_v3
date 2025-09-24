using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.CompCard.Vm;

public partial class CdFieldVm : BaseVmForFieldWithButton
{
    public CdFieldVm(Action openWindow) : base(openWindow) {
        _label = "Условное обозначение: ";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }
}

public abstract class BaseVmForFieldWithButton : ObservableObject
{
    protected readonly Action _openWindow;
    protected string _value;
    protected string _label;

    protected BaseVmForFieldWithButton(Action openWindow) {
        _openWindow = openWindow;
    }

    public string Value {
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
    }

    protected abstract void OpenNestedWindow();
}
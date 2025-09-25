using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.CompCard.Vm;

public partial class ManFieldVm : BaseVmForFieldWithButton
{
    public ManFieldVm(Action openWindow) : base(openWindow) {
        _label = "Производитель: ";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }
}
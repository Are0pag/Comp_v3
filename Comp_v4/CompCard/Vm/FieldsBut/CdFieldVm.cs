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
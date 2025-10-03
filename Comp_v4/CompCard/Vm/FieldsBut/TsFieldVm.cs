using CommunityToolkit.Mvvm.Input;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public partial class TsFieldVm : BaseVmForFieldWithButton
{
    public TsFieldVm(Action openWindow) : base(openWindow) {
        _label = "Типоразмер: ";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }
}
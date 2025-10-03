using CommunityToolkit.Mvvm.Input;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public partial class CdFieldVm : BaseVmForFieldWithButton
{
    public CdFieldVm(StringValidatorBase validator, Action openWindow) : base(validator, openWindow) {
        _label = "Условное обозначение: ";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }
}
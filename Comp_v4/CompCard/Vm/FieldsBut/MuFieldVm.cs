using CommunityToolkit.Mvvm.Input;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public partial class MuFieldVm : BaseVmForFieldWithButton
{
    public MuFieldVm(StringValidatorBase validator, Action openWindow) : base(validator, openWindow) {
        _label = "Единица измерения: ";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }
}
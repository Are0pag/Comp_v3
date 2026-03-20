using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.CompCard.Vm;

public partial class GpsFieldVm : BaseVmForFieldWithButton<GenericParametersSet>
{
    public GpsFieldVm(Action openWindow) : base(openWindow) {
        _label = "Набор параметров: ";
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.GenericParametersSet?.Name ?? "...";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }

    public override void HandleTableInput(GenericParametersSet? args) {
        Value = args?.Name ?? "...";
    }
}
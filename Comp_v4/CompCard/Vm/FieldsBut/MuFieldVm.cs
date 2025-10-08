using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public partial class MuFieldVm : BaseVmForFieldWithButton<MeasurementUnit>
{
    public MuFieldVm(Action openWindow) : base(openWindow) {
        _label = "Единица измерения: ";
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.MeasurementUnit?.Designation ?? "...";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }

    public override void HandleTableInput(MeasurementUnit? args) {
        Value = args?.Designation ?? "...";
        OnPropertyChanged(nameof(Value));
    }
}
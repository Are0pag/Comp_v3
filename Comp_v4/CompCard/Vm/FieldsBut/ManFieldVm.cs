using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.CompCard.Vm;

public partial class ManFieldVm : BaseVmForFieldWithButton<Manufacturer>
{
    public ManFieldVm(Action openWindow) : base(openWindow) {
        _label = "Производитель: ";
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.Manufacturer?.Designation ?? "...";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }

    public override void HandleTableInput(Manufacturer? args) {
        Value = args?.Designation ?? "...";
        OnPropertyChanged(nameof(Value));
    }
}
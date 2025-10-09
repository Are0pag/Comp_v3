using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.CompCard.Vm;

public partial class CdFieldVm : BaseVmForFieldWithButton<ConditionalDesignation>
{
    public CdFieldVm(Action openWindow) : base(openWindow) {
        _label = "Условное обозначение: ";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }

    public override void HandleTableInput(ConditionalDesignation? args) {
        Value = args?.Designation ?? "...";
        OnPropertyChanged(nameof(Value));
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.ConditionalDesignation?.Designation ?? "...";
    }

}
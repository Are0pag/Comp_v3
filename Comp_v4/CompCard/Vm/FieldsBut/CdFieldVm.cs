using CommunityToolkit.Mvvm.Input;
using Comp_v4.CompCard.Events;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Services.ValidationString;

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
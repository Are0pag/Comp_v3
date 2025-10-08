using CommunityToolkit.Mvvm.Input;
using Comp_v4.CompCard.Events;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public partial class CdFieldVm : BaseVmForFieldWithButton, IExternalTableInputHandler, ICompCardLoadedHandler
{
    public CdFieldVm(Action openWindow) : base(openWindow) {
        _label = "Условное обозначение: ";
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }

    public void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public void OnCompCardLoaded(Component component) {
        _value = component.ConditionalDesignation?.Designation ?? "...";
    }

    public void HandleTableInput(object? args) {
        if (args is not ConditionalDesignation cd) 
            return;
        Value = cd.Designation;
        OnPropertyChanged(nameof(Value));
    }
}
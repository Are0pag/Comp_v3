using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.CompCard.Vm;

public partial class TsFieldVm : BaseVmForFieldWithButton<TypeSize>
{
    public TsFieldVm(Action openWindow) : base(openWindow) {
        _label = "Типоразмер: ";
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.TypeSize?.Designation ?? "...";
    }

    [RelayCommand]
    protected override void OpenNestedWindow() {
        _openWindow.Invoke();
    }

    public override void HandleTableInput(TypeSize? args) {
        Value = args?.Designation ?? "...";
        OnPropertyChanged(nameof(Value));
    }
}
using Comp_v4.CompCard.Vm.Buttons;
using Comp.ModelData.Comp;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.CompCard.Operations.Actions;

public class SaveComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly Component _component;
    public SaveComponentAction(SaveCompButtonVm buttonVm, Component component) : base(buttonVm) {
        _component = component;
    }

    public override async Task PerformAsync(object? parameter) {
        throw new NotImplementedException();
    }

    public override bool CanPerform() {
        throw new NotImplementedException();
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
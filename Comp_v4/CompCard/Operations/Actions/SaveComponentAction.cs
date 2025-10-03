using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Vm.Buttons;
using Comp.ModelData.Comp;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.CompCard.Operations.Actions;

public class SaveComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly Component _component;
    protected readonly ValidatorCardComp _validator;
    public SaveComponentAction(SaveCompButtonVm buttonVm, Component component, ValidatorCardComp validator) : base(buttonVm) {
        _component = component;
        _validator = validator;
    }

    public override async Task PerformAsync(object? parameter) {
        throw new NotImplementedException();
    }

    public override bool CanPerform() {
        return _validator.IsValid();
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
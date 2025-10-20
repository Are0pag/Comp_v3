using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Vm.Buttons;
using Utils.WPF;

namespace Comp_v4.CompCard.Operations.Actions;

public class SaveComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly CardComp _cardOfComponent;
    protected readonly CardCopmEditController _editController;
    public SaveComponentAction(SaveCompButtonVm buttonVm,  CardCopmEditController editController, CardComp cardOfComponent) : base(buttonVm) {
        _editController = editController;
        _cardOfComponent = cardOfComponent;
    }

    public override Task PerformAsync(object? parameter) {
        _cardOfComponent.Save();
        return Task.CompletedTask;
    }

    public override bool CanPerform() {
        return _editController.IsValid();
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
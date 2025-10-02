using Comp_v4.CompCard.Entities;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Operations.Actions.Components;

public class AddComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly CardComp _cardComp;
    public AddComponentAction(BaseAsyncBButtonVm buttonVm, CardComp cardComp) : base(buttonVm) {
        _cardComp = cardComp;
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
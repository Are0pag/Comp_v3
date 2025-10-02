using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp.ModelData.Comp;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Operations.Actions.Components;

public class AddComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly CardComponentManager _cardComponentManager;
    public AddComponentAction(BaseAsyncBButtonVm buttonVm, CardComponentManager cardComponentManager) : base(buttonVm) {
        _cardComponentManager = cardComponentManager;
    }

    public override async Task PerformAsync(object? parameter) {
        /*if (parameter is not Component component)
            throw new InvalidCastException();*/
        
        _cardComponentManager.OpenWindow<CreateStateCardComp>(new Component());
    }

    public override bool CanPerform() {
        throw new NotImplementedException();
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
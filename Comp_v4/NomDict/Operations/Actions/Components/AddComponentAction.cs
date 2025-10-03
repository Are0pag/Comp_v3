using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons.Components;
using Comp.ModelData.Comp;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Operations.Actions.Components;

public class AddComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly CardComponentManager _cardComponentManager;
    protected readonly TreeViewVm _treeViewVm;
    public AddComponentAction(AddCompButtonVm buttonVm, CardComponentManager cardComponentManager, TreeViewVm treeViewVm) : base(buttonVm) {
        _cardComponentManager = cardComponentManager;
        _treeViewVm = treeViewVm;
    }

    public override async Task PerformAsync(object? parameter) {
        if (_treeViewVm.SelectedCategory == null)
            return;
        
        _cardComponentManager.OpenWindow<CreateStateCardComp>(new CardComponentManager.Args(
                                                                   new Component(),
                                                                   _treeViewVm.SelectedCategory
                                                                   ));
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
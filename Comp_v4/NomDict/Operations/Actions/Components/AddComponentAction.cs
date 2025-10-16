using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons.Components;
using Utils.WPF;

namespace Comp_v4.NomDict.Operations.Actions.Components;

public class AddComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly Grid _grid;
    protected readonly TreeViewVm _treeViewVm;
    public AddComponentAction(AddCompButtonVm buttonVm, TreeViewVm treeViewVm, Grid grid) : base(buttonVm) {
        _treeViewVm = treeViewVm;
        _grid = grid;
    }

    public override async Task PerformAsync(object? parameter) {
        _grid.AddComponent(parameter);
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
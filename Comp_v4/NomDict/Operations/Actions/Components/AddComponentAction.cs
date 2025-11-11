using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons.Components;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Operations.Actions.Components;

public class AddComponentAction : BaseActionAsyncSelfWaiting
{
    protected readonly Grid _grid;
    protected readonly TreeViewVm _treeViewVm;
    public AddComponentAction(AddCompButtonVm button, Grid grid, TreeViewVm treeViewVm) : base(button) {
        _grid = grid;
        _treeViewVm = treeViewVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _grid.AddComponent(tcs: tcs, parameter: null);
    }

    public override bool CanPerform() {
        return base.CanPerform() && _treeViewVm.SelectedCategory != null;
    }
}
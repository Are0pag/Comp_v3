using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Vm.Buttons.Components;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Operations.Actions.Components;

public class AddComponentAction : BaseActionAsyncSelfWaiting
{
    protected readonly Grid _grid;
    public AddComponentAction(AddCompButtonVm button, Grid grid) : base(button) {
        _grid = grid;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _grid.AddComponent(tcs: tcs, parameter: null);
    }
}
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Grid.States;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;

public class ChangeTargetStateCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public ChangeTargetStateCommand(CognDesignGridVm context) : base(context) {
    }

    public override async Task ExecuteAsync() {
        await _context.StateProvider.ChangeState(_context.StateProvider.GetState<StateDgCreatingNewItem>(), _context);
    }

    public override async Task UndoAsync() {
        await _context.StateProvider.ChangeState(_context.StateProvider.GetState<StateDgEditing>(), _context);
    }

    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}
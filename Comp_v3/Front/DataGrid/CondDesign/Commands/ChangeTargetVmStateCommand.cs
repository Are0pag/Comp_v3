using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Grid.States;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class ChangeTargetVmStateCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    protected readonly StateDataGrid _fromState;
    protected readonly StateDataGrid _targetState;
    
    public ChangeTargetVmStateCommand(CognDesignGridVm context, StateDataGrid fromState, StateDataGrid targetState) 
        : base(context) 
    {
        _fromState = fromState;
        _targetState = targetState;
    }

    public override async Task ExecuteAsync() {
        await _context.StateProvider.ChangeState(_targetState, _context);
    }

    public override async Task UndoAsync() {
        await _context.StateProvider.ChangeState(_fromState, _context);
    }

    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}
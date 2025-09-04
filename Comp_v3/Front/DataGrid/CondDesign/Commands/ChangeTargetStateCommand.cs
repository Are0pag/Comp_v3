using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Grid.States;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class ChangeTargetStateCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    protected readonly StateDataGrid _fromState;
    protected readonly StateDataGrid _targetState;
    
    public ChangeTargetStateCommand(CognDesignGridVm context, StateDataGrid fromState, StateDataGrid targetState) 
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

/*public class ChangeTargetStateCommand<TTarget, TFrom> : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
    where TFrom : StateDataGrid
    where TTarget : StateDataGrid
{
    protected readonly TFrom _previousState;
    protected readonly TTarget _targetState;
    
    public ChangeTargetStateCommand(CognDesignGridVm context, TFrom previousState, TTarget targetState) : base(context) {
        _previousState = previousState;
        _targetState = targetState;
    }

    public override async Task ExecuteAsync() {
        await _context.StateProvider.ChangeState(_context.StateProvider.GetState<TTarget>(), _context);
    }

    public override async Task UndoAsync() {
        await _context.StateProvider.ChangeState(_context.StateProvider.GetState<TFrom>(), _context);
    }

    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}*/
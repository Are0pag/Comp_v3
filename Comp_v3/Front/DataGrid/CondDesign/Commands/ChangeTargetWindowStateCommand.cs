using Comp_v3.Front.DataGrid.CondDesign.Window;
using Comp_v3.Front.DataGrid.CondDesign.Window.States;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class ChangeTargetWindowStateCommand : DeferredCommand<CognDesignGridWindow, ConditionalDesignation>
{
    protected readonly StateWindow _fromState;
    protected readonly StateWindow _targetState;
    
    public ChangeTargetWindowStateCommand(CognDesignGridWindow context, StateWindow fromState, StateWindow targetState) 
        : base(context) 
    {
        _fromState = fromState;
        _targetState = targetState;
    }

    public override Task ExecuteAsync() {
        _context.StateProvider.SwitchStateWindow(_targetState, _context);
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.StateProvider.RollbackStateWindow(_fromState, _context);
        return Task.CompletedTask;
    }

    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}
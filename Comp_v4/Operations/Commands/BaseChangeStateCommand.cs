using WPF.Templates;
using WPF.Templates.TableWindow.States;

namespace Comp_v4.Operations.Commands;

public abstract class BaseChangeStateCommand<TBaseState, TStateMachine> : BaseCommand
    where TBaseState : Infrastructure.StateMachine.StateBase<Cell>
    where TStateMachine : Infrastructure.StateMachine.GenericStateMachine<TBaseState, Cell>
{
    protected readonly TStateMachine _stateMachine;
    protected readonly TBaseState _fromState;
    protected readonly TBaseState _targetState;

    public BaseChangeStateCommand(ModuleContext context, TStateMachine stateMachine, TBaseState targetState) : base(context) {
        _stateMachine = stateMachine;
        _fromState = stateMachine.CurrentState;
        _targetState = targetState;
    }

    public override async Task ExecuteAsync() {
        await _stateMachine.ChangeState(_targetState, null);
    }

    public override async Task UndoAsync() {
        await _stateMachine.RollbackState(_fromState, null);
    }
    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}

public class CellChangeStateCommand : BaseChangeStateCommand<BaseCellState, Cell>
{
    public CellChangeStateCommand(ModuleContext context, Cell stateMachine, BaseCellState targetState) : base(context, stateMachine, targetState) {
    }
}
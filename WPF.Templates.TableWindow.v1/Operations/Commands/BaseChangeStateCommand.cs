using System.Windows;
using WPF.Templates;
using WPF.Templates.TableWindow.States;

namespace Comp_v4.Operations.Commands;

public abstract class BaseChangeStateCommand<TWindow, T, TBaseState, TStateMachine> : BaseCommand<object>
    where TWindow : Window
    where T : class
    where TBaseState : Infrastructure.StateMachine.StateBase<Cell<TWindow, T>>
    where TStateMachine : Infrastructure.StateMachine.GenericStateMachine<TBaseState, Cell<TWindow, T>>
{
    protected readonly TStateMachine _stateMachine;
    protected readonly TBaseState _fromState;
    protected readonly TBaseState _targetState;

    public BaseChangeStateCommand(object parameter, TStateMachine stateMachine, TBaseState targetState) : base(parameter) {
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


public class CellChangeStateCommand<TWindow, T> : BaseChangeStateCommand<TWindow, T, BaseCellState<TWindow, T>, Cell<TWindow, T> >
    where TWindow : Window
    where T : class
{
    /*public CellChangeStateCommand(ModuleContext context, Cell stateMachine, BaseCellState targetState) : base(context, stateMachine, targetState) {
    }*/
    public CellChangeStateCommand(ModuleContext<TWindow, T> parameter, Cell<TWindow, T> stateMachine, BaseCellState<TWindow, T> targetState) 
        : base(parameter, stateMachine, targetState) {
    }
}
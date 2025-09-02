using Infrastructure.Command.Classic;
using Infrastructure.StateMachine;

namespace Infrastructure.Command.StateCombine;

public class CommandStateMachine<TState, TContext> : GenericStateMachine<TState, TContext>
    where TState : class, ICommandState<TContext>
    where TContext : class 
{
    private readonly AdvancedUndoRedoManager _undoManager;
    private readonly Stack<TState> _stateHistory = new();

    public CommandStateMachine(IEnumerable<TState> states, 
                               TState initialState,
                               AdvancedUndoRedoManager undoManager) : base(states, initialState) {
        _undoManager = undoManager;
    }

    public override async Task ChangeState(TState newState, TContext context) {
        var exitCommand = await CurrentState.GetExitCommand(context).ConfigureAwait(false);
        var enterCommand = await newState.GetEnterCommand(context).ConfigureAwait(false);

        // Создаем композитную команду для смены состояния
        var stateChangeCommand = new StateChangeCommand(exitCommand, enterCommand, CurrentState, newState);

        await _undoManager.ExecuteCommand(stateChangeCommand).ConfigureAwait(false);
        _stateHistory.Push(CurrentState);
        
        CurrentState = newState;
    }

    public async Task GoBack(TContext context) {
        if (_stateHistory.Count == 0) return;
        
        var previousState = _stateHistory.Pop();
        await ChangeState(previousState, context).ConfigureAwait(false);
    }

    private class StateChangeCommand : ICommand {
        private readonly ICommand _exitCommand;
        private readonly ICommand _enterCommand;
        private readonly TState _oldState;
        private readonly TState _newState;

        public string Description => $"State change: {_oldState.GetType().Name} → {_newState.GetType().Name}";

        public StateChangeCommand(ICommand exitCommand, ICommand enterCommand, 
                                  TState oldState, TState newState) {
            _exitCommand = exitCommand;
            _enterCommand = enterCommand;
            _oldState = oldState;
            _newState = newState;
        }

        public async Task ExecuteAsync() {
            await _exitCommand.ExecuteAsync().ConfigureAwait(false);
            await _enterCommand.ExecuteAsync().ConfigureAwait(false);
        }

        public async Task UndoAsync() {
            await _enterCommand.UndoAsync().ConfigureAwait(false);
            await _exitCommand.UndoAsync().ConfigureAwait(false);
        }
    }
}
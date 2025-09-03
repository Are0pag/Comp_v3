namespace Infrastructure.Command.Base;

/* Redo для транзакций не реализован */
public class CommandScheduler<TCommand> : ICommandScheduler<TCommand> 
    where TCommand : ICommand
{
    protected readonly Stack<TCommand> _undoStack = new();
    protected readonly Stack<TCommand> _redoStack = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;
    
    public enum CommandAction { Executed, Undone, Redone }
    public Action<CommandAction, TCommand>? OnCommandExecuted { get; set; }

    public virtual async Task ExecuteCommand(TCommand command) {
        await command.ExecuteAsync();
        _undoStack.Push(command);
        _redoStack.Clear();
        OnCommandExecuted?.Invoke(CommandAction.Executed, command);
    }

    public virtual async Task UndoAsync() {
        if (!CanUndo) return;
        var command = _undoStack.Pop();
        await command.UndoAsync();
        _redoStack.Push(command);
        OnCommandExecuted?.Invoke(CommandAction.Undone, command);
    }

    public virtual async Task RedoAsync() {
        if (!CanRedo) return;
        
        var command = _redoStack.Pop();
        await command.ExecuteAsync();
        _undoStack.Push(command);
        OnCommandExecuted?.Invoke(CommandAction.Redone, command);
    }
}
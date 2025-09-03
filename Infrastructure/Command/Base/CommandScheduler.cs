namespace Infrastructure.Command.Base;

/* Redo для транзакций не реализован */
public class CommandScheduler<TCommand> : ICommandScheduler<TCommand> 
    where TCommand : ICommand
{
    protected readonly Stack<TCommand> _undoStack = new();
    protected readonly Stack<TCommand> _redoStack = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public virtual async Task ExecuteCommand(TCommand command) {
        await command.ExecuteAsync();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public virtual async Task UndoAsync() {
        if (!CanUndo) return;
        var command = _undoStack.Pop();
        await command.UndoAsync();
        _redoStack.Push(command);
    }

    public virtual async Task RedoAsync() {
        if (!CanRedo) return;
        
        var command = _redoStack.Pop();
        await command.ExecuteAsync();
        _undoStack.Push(command);
    }
}
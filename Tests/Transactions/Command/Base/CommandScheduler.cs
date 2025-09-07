namespace Infrastructure.Command.Base;

public class CommandScheduler<T> : ICommandScheduler<T> 
    where T : ICommand
{
    protected readonly Stack<T> _undoStack = new();
    protected readonly Stack<T> _redoStack = new();

    public virtual bool CanUndo() => _undoStack.Count > 0;
    public virtual bool CanRedo() => _redoStack.Count > 0;
    
    public enum CommandAction { Executed, Undone, Redone }
    public Action<CommandAction, T>? OnCommandExecuted { get; set; }

    public virtual async Task ExecuteCommand(T command) {
        await command.ExecuteAsync();
        _undoStack.Push(command);
        _redoStack.Clear();
        OnCommandExecuted?.Invoke(CommandAction.Executed, command);
    }

    public virtual async Task UndoAsync() {
        if (!CanUndo()) return;
        var command = _undoStack.Pop();
        await command.UndoAsync();
        _redoStack.Push(command);
        OnCommandExecuted?.Invoke(CommandAction.Undone, command);
    }

    public virtual async Task RedoAsync() {
        if (!CanRedo()) return;
        
        var command = _redoStack.Pop();
        await command.ExecuteAsync();
        _undoStack.Push(command);
        OnCommandExecuted?.Invoke(CommandAction.Redone, command);
    }
}
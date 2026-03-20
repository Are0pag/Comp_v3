namespace Infrastructure.Command.Base;

public class CommandScheduler<T> : ICommandScheduler<T> 
    where T : ICommand
{
    protected readonly Stack<T> _undoStack = new();
    protected readonly Stack<T> _redoStack = new();

    public bool CanContinueWorking { get; private set; } = true;
    
    public virtual bool CanUndo() => _undoStack.Count > 0;
    public virtual bool CanRedo() => _redoStack.Count > 0;
    
    public enum CommandAction { Executed, Undone, Redone }
    public Action<CommandAction, T>? OnCommandExecuted { get; set; }

    public virtual async Task<ICommandScheduler<T>> ExecuteCommand(T command) {
        if (!CanContinueWorking) return this;
        await command.ExecuteAsync();
        _undoStack.Push(command);
        _redoStack.Clear();
        OnCommandExecuted?.Invoke(CommandAction.Executed, command);
        return this;
    }

    public virtual async Task<object> UndoAsync() {
        if (!CanUndo())
            return this;
        
        CanContinueWorking = false;
        
        var command = _undoStack.Pop();
        await command.UndoAsync();
        _redoStack.Push(command);
        
        CanContinueWorking = true;
        OnCommandExecuted?.Invoke(CommandAction.Undone, command);
        return command;
    }

    public virtual async Task<object> RedoAsync() {
        if (!CanRedo()) 
            throw new InvalidOperationException();
        
        var command = _redoStack.Pop();
        await command.ExecuteAsync();
        _undoStack.Push(command);
        OnCommandExecuted?.Invoke(CommandAction.Redone, command);
        return command;
    }
}
using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public abstract class TransactionBase<T> : ITransaction<T>
    where T : ICommand
{
    protected readonly List<T> _commands = new();
    
    public ITransaction<T> AddCommand(T command) {
        _commands.Add(command);
        return this;
    }

    public ITransaction<T> RemoveCommand(T command) {
        _commands.Remove(command);
        return this;
    }

    public IEnumerable<T> GetCommands() => _commands.AsReadOnly();

    public string? Description { get; set; }

    public virtual async Task ExecuteAsync() {
        foreach (var command in _commands) 
            await command.ExecuteAsync().ConfigureAwait(false);
    }

    public virtual async Task UndoAsync() {
        for (var i = _commands.Count - 1; i >= 0; i--) 
            await _commands[i].UndoAsync().ConfigureAwait(false);
    }

    public bool IsExecuted { get; protected set; }

    public ICommand MarkAs(bool isExecuted) {
        IsExecuted = isExecuted;
        return this;
    }
}

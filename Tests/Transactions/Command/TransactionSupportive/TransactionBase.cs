using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public abstract class TransactionBase<T> : ITransaction<T>
    where T : ICommand
{
    protected readonly List<T> _commands = new();
    public void AddCommand(T command) => _commands.Add(command);
    public void RemoveCommand(T command) => _commands.Remove(command);
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
}

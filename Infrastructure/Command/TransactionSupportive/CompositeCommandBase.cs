using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public abstract class CompositeCommandBase : ICompositeCommand
{
    private readonly List<ICommand> _commands = new();
    public void AddCommand(ICommand command) => _commands.Add(command);
    public void RemoveCommand(ICommand command) => _commands.Remove(command);
    public IEnumerable<ICommand> GetCommands() => _commands.AsReadOnly();

    public virtual async Task ExecuteAsync() {
        foreach (var command in _commands) 
            await command.ExecuteAsync().ConfigureAwait(false);
    }

    public virtual async Task UndoAsync() {
        for (int i = _commands.Count - 1; i >= 0; i--) 
            await _commands[i].UndoAsync().ConfigureAwait(false);
    }
}

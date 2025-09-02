namespace Infrastructure.Command.Classic;

public interface ICompositeCommand : ICommand
{
    public abstract void AddCommand(ICommand command);
    public abstract void RemoveCommand(ICommand command);
    public abstract IEnumerable<ICommand> GetCommands();
}
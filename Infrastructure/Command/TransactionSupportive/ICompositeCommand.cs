namespace Infrastructure.Command.Classic;

public interface ICompositeCommand : ICommand
{
    void AddCommand(ICommand command);
    void RemoveCommand(ICommand command);
    IEnumerable<ICommand> GetCommands();
}
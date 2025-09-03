using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public interface ICompositeCommand : ICommand
{
    void AddCommand(ICommand command);
    void RemoveCommand(ICommand command);
    IEnumerable<ICommand> GetCommands();
}
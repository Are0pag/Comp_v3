using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public interface ITransaction<T>
    where T : ICommand
{
    void AddCommand(T command);
    void RemoveCommand(T command);
    IEnumerable<T> GetCommands();
}


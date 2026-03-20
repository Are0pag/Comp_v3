using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public interface ITransaction<T> : ICommand
    where T : ICommand
{
    ITransaction<T> AddCommand(T command);
    ITransaction<T> RemoveCommand(T command);
    IEnumerable<T> GetCommands();
}


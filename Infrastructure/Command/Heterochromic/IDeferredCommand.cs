using Infrastructure.Command.Base;
using Infrastructure.Command.TransactionSupportive;

namespace Infrastructure.Command.Heterochromic;

public interface IDeferredCommand : ICommand
{
    Task ExecuteDeferredAsync();
}

/*public interface ITransactionDeferredSupportive<T> : ITransaction<T>
    where T : IDeferredCommand
{
    Task ExecuteDeferredAsync();
}*/

public class TransactionDeferredSupportive : TransactionBase<IDeferredCommand>
{
    public async Task ExecuteDeferredAsync() {
        foreach (var deferred in _commands)
            await deferred.ExecuteDeferredAsync();
    }
}
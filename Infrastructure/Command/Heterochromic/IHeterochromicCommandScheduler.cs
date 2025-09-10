using Infrastructure.Command.TransactionSupportive;

namespace Infrastructure.Command.Heterochromic;

public interface IHeterochromicCommandScheduler<T, TTransaction> : ITransactionalCommandScheduler<T, TTransaction>
    where T : IDeferredCommand
    where TTransaction : ITransaction<T>, new()
{
    Task CommitDeferredChanges();
}
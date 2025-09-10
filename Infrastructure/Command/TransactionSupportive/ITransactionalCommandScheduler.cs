using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public interface ITransactionalCommandScheduler<T, TTransaction> : ICommandScheduler<T>
    where T : ICommand
    where TTransaction : ITransaction<T>, new()
{
    bool IsInAnyTransaction { get; }
    
    bool IsInTransaction<TCurrentTransaction>() where TCurrentTransaction : TTransaction;

    TransactionalCommandScheduler<T, TTransaction> BeginTransaction<TCurrentTransaction>(string? descr = null) where TCurrentTransaction : TTransaction, new();

    TransactionalCommandScheduler<T, TTransaction> RegisterCommand(T command);
    TransactionalCommandScheduler<T, TTransaction> RegisterCommandInto<TCurrentTransaction>(T command) where TCurrentTransaction : TTransaction;

    TransactionalCommandScheduler<T, TTransaction> CommitTransaction<TCurrentTransaction>() where TCurrentTransaction : TTransaction;
    
    Task<T> ExecuteLastRegisteredAsync();
}

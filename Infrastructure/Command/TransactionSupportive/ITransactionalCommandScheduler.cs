using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public interface ITransactionalCommandScheduler<T, TTransaction> : ICommandScheduler<T> 
    where T : ICommand
    where TTransaction : ITransaction<T>, new()
{
    Task<TransactionalCommandScheduler<T, TTransaction>> BeginTransactionAsync<TCurrentTransaction>(T? command = default, bool executeNow = true, string? descr = null)
        where TCurrentTransaction : TTransaction, new();

    Task<TransactionalCommandScheduler<T, TTransaction>> ContinueTransactionAsync<TCurrentTransaction>(T command, bool executeNow = true)
        where TCurrentTransaction : TTransaction;

    public TransactionalCommandScheduler<T, TTransaction> CommitTransaction<TCurrentTransaction>()
        where TCurrentTransaction : TTransaction;
    
    Task<TransactionalCommandScheduler<T, TTransaction>> ExecuteTransactionAsync<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction;
}
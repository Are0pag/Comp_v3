using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public class TransactionalCommandScheduler<T, TTransaction> : CommandScheduler<T>
    where T : ICommand
    where TTransaction : ITransaction<T>, new()
{
    protected readonly Dictionary<Type, TTransaction> _creatingTransactions = new Dictionary<Type, TTransaction>();
    
    public async Task<TransactionalCommandScheduler<T, TTransaction>> BeginTransactionAsync<TCurrentTransaction>(T? command = default, bool executeNow = true, string? descr = null) 
        where TCurrentTransaction : TTransaction, new() 
    {
        var transaction = new TCurrentTransaction() {
            Description = descr
        };
        _creatingTransactions.Add(typeof(TCurrentTransaction), transaction);
        
        if (command != null) 
            transaction.AddCommand(command);
        
        if (executeNow) 
            await transaction.ExecuteAsync();
        return this; 
    }

    public async Task<TransactionalCommandScheduler<T, TTransaction>> ContinueTransactionAsync<TCurrentTransaction>(T command, bool executeNow = true)
        where TCurrentTransaction : TTransaction
    {
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");
        transaction.AddCommand(command);
        if (executeNow) await command.ExecuteAsync();
        return this;
    }

    public TransactionalCommandScheduler<T, TTransaction> CommitTransaction<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction
    {
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");

        if (!transaction.GetCommands().Any()) 
            throw new InvalidOperationException("The transaction has not any commands");
        
        _creatingTransactions.Remove(typeof(TCurrentTransaction));
        
        if (transaction is not T typedTransaction) 
            throw new InvalidOperationException("Transaction cannot be added to undo stack");
        
        _undoStack.Push(typedTransaction);
        _redoStack.Clear();
        return this;
    }

    /*public async Task<TransactionalCommandScheduler<T, TTransaction>> ExecuteTransactionAsync<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction
    {
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");

        foreach (var command in transaction.GetCommands()) {
            await command.ExecuteAsync();
        }
        return this;
    }*/

    public override async Task<object> UndoAsync() {
        if (!CanUndo()) 
            throw new InvalidOperationException("Cannot undo");
        
        var result = await base.UndoAsync();
        return result; // потом у команды можно взять тип
    }
}
using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public class TransactionalCommandScheduler<T, TTransaction> : CommandScheduler<T>
    where T : ICommand
    where TTransaction : ITransaction<T>, new()
{
    protected readonly Dictionary<Type, TTransaction> _creatingTransaction = new Dictionary<Type, TTransaction>();
    
    public TransactionalCommandScheduler<T, TTransaction> BeginTransactionAsync<TCurrentTransaction>(T? command = default, string? descr = null) 
        where TCurrentTransaction : TTransaction, new() 
    {
        if (_creatingTransaction.ContainsKey(typeof(TCurrentTransaction))) 
            throw new InvalidOperationException("Transaction already exists");
        
        var transaction = new TCurrentTransaction() {
            Description = descr
        };
        _creatingTransaction.Add(typeof(TCurrentTransaction), transaction);
        if (command != null) transaction.AddCommand(command);
        return this; 
    }

    public TransactionalCommandScheduler<T, TTransaction> ContinueTransactionAsync<TCurrentTransaction>(T command)
        where TCurrentTransaction : TTransaction
    {
        if (!_creatingTransaction.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");
        transaction.AddCommand(command);
        return this;
    }

    public async Task<TransactionalCommandScheduler<T, TTransaction>> CommitTransaction<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction
    {
        if (!_creatingTransaction.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");

        if (!transaction.GetCommands().Any()) 
            throw new InvalidOperationException("The transaction has not any commands");

        if (transaction is not T typedTransaction) 
            throw new InvalidOperationException("Transaction cannot be added to undo stack");

        await transaction.ExecuteAsync();
        _creatingTransaction.Remove(typeof(TCurrentTransaction));
        _undoStack.Push(typedTransaction);
        _redoStack.Clear();
        return this;
    }

    public override async Task<object> UndoAsync() {
        if (!CanUndo()) 
            throw new InvalidOperationException("Cannot undo");
        
        var result = await base.UndoAsync();
        return result; // потом у команды можно взять тип
    }
}
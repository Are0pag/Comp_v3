using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public class TransactionalCommandScheduler<T, TTransaction> : CommandScheduler<T>
    where T : ICommand
    where TTransaction : ITransaction<T>, new()
{
    protected readonly Dictionary<Type, TTransaction> _creatingTransactions = new Dictionary<Type, TTransaction>();
    
    public T? LastRegistered { get; protected set; }
    
    public TransactionalCommandScheduler<T, TTransaction> BeginTransaction<TCurrentTransaction>(string? descr = null) 
        where TCurrentTransaction : TTransaction, new() 
    {
        if (_creatingTransactions.ContainsKey(typeof(TCurrentTransaction))) 
            throw new InvalidOperationException("Transaction already exists");
        
        var transaction = new TCurrentTransaction() {
            Description = descr
        };
        _creatingTransactions.Add(typeof(TCurrentTransaction), transaction);
        return this; 
    }

    public TransactionalCommandScheduler<T, TTransaction> RegisterCommandInto<TCurrentTransaction>(T command)
        where TCurrentTransaction : TTransaction
    {
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");
        transaction.AddCommand(command);
        LastRegistered = command;
        return this;
    }

    public async Task<TransactionalCommandScheduler<T, TTransaction>> CommitTransaction<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction
    {
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");

        if (!transaction.GetCommands().Any()) 
            throw new InvalidOperationException("The transaction has not any commands");

        if (transaction is not T typedTransaction) 
            throw new InvalidOperationException("Transaction cannot be added to undo stack");

        await transaction.ExecuteAsync();
        _creatingTransactions.Remove(typeof(TCurrentTransaction));
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
using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public class TransactionalCommandScheduler<T, TTransaction> : CommandScheduler<T>
    where T : ICommand
    where TTransaction : ITransaction<T>, new()
{
    protected readonly Dictionary<Type, TTransaction> _creatingTransactions = new Dictionary<Type, TTransaction>();
    protected Type? _lastCreatingTransaction; /* context field for convenience */

    public virtual bool IsInAnyTransaction => _creatingTransactions.Count != 0;
    public virtual bool IsInTransaction<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction 
    {
        return _creatingTransactions.ContainsKey(typeof(TCurrentTransaction));
    }

    public virtual TransactionalCommandScheduler<T, TTransaction> BeginTransaction<TCurrentTransaction>(string? descr = null) 
        where TCurrentTransaction : TTransaction, new() 
    {
        if (!CanContinueWorking) return this;
        if (_creatingTransactions.ContainsKey(typeof(TCurrentTransaction))) 
            throw new InvalidOperationException("Transaction already exists");
        
        var transaction = new TCurrentTransaction() {
            Description = descr
        };
        _creatingTransactions.Add(typeof(TCurrentTransaction), transaction);
        _lastCreatingTransaction = typeof(TCurrentTransaction);
        return this; 
    }

    public virtual TransactionalCommandScheduler<T, TTransaction> RegisterCommand(T command) {
        if (!CanContinueWorking) return this;
        if (_lastCreatingTransaction == null)
            throw new InvalidOperationException("Do not call BeginTransaction before calling BeginTransaction");
        
        if (!_creatingTransactions.TryGetValue(_lastCreatingTransaction!, out var transaction))
            throw new InvalidOperationException("The transaction was not created");
        
        transaction.AddCommand(command);
        return this;
    }
    
    public virtual TransactionalCommandScheduler<T, TTransaction> RegisterCommandInto<TCurrentTransaction>(T command) 
        where TCurrentTransaction : TTransaction
    {
        if (!CanContinueWorking) return this;
        
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created");
        
        transaction.AddCommand(command);
        _lastCreatingTransaction = typeof(TCurrentTransaction);
        return this;
    }

    public virtual async Task<T> ExecuteLastRegisteredAsync() {
        if (!CanContinueWorking) return default;
        if (_lastCreatingTransaction == null)
            throw new InvalidOperationException("Do not call BeginTransaction before calling BeginTransaction");
        
        if (!_creatingTransactions.TryGetValue(_lastCreatingTransaction!, out var transaction))
            throw new InvalidOperationException("The transaction was not created");
        
        if (!transaction.GetCommands().Any())
            throw new InvalidOperationException("The transaction has not any registered commands");

        var command = transaction.GetCommands().Last();
        await command.ExecuteAsync();
        return command;
    }

    public virtual TransactionalCommandScheduler<T, TTransaction> CommitTransaction<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction
    {
        if (!CanContinueWorking) return this;
        
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            throw new InvalidOperationException("The transaction was not created or already commited");

        if (!transaction.GetCommands().Any()) 
            throw new InvalidOperationException("The transaction has not any commands");

        if (transaction is not T typedTransaction) 
            throw new InvalidOperationException("Transaction cannot be added to undo stack");
        
        _creatingTransactions.Remove(typeof(TCurrentTransaction));
        _undoStack.Push(typedTransaction);
        _redoStack.Clear();
        return this;
    }

    public override async Task<object> UndoAsync() {
        if (!CanUndo()) return default;
        
        var result = await base.UndoAsync();
        return result; // потом у команды можно взять тип
    }
}
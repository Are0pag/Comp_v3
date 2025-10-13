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
        if (!CanContinueWorking) 
            return this;
        if (_creatingTransactions.ContainsKey(typeof(TCurrentTransaction))) 
            new InvalidOperationException("Transaction already exists").Log(this);
        
        var transaction = new TCurrentTransaction() {
            Description = descr
        };
        _creatingTransactions.Add(typeof(TCurrentTransaction), transaction);
        _lastCreatingTransaction = typeof(TCurrentTransaction);
        #if DEBUG
            Console.WriteLine($"scheduler: Begin: {transaction.ToString()}");
        #endif
        return this; 
    }

    public virtual TransactionalCommandScheduler<T, TTransaction> RegisterCommand(T command) {
        if (!CanContinueWorking) 
            return this;
        if (_lastCreatingTransaction == null)
            throw new InvalidOperationException("Do not call BeginTransaction before calling BeginTransaction");
        
        if (!_creatingTransactions.TryGetValue(_lastCreatingTransaction!, out var transaction))
            new InvalidOperationException("The transaction was not created").Log(this);
        
        transaction.AddCommand(command);
        #if DEBUG
            Console.WriteLine($"scheduler: Register {command.ToString()} to {transaction.ToString()}");
        #endif
        return this;
    }
    
    public virtual TransactionalCommandScheduler<T, TTransaction> RegisterCommandInto<TCurrentTransaction>(T command) 
        where TCurrentTransaction : TTransaction
    {
        if (!CanContinueWorking) return this;
        
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            new InvalidOperationException("The transaction was not created").Log(this);
        
        transaction.AddCommand(command);
        _lastCreatingTransaction = typeof(TCurrentTransaction);
    #if DEBUG
        Console.WriteLine($"scheduler: Register {command.ToString()} to {transaction.ToString()}");
    #endif
        return this;
    }

    public virtual async Task<T> ExecuteLastRegisteredAsync() {
        if (!CanContinueWorking) return default;
        if (_lastCreatingTransaction == null)
            new InvalidOperationException("Do not call BeginTransaction before calling BeginTransaction").Log(this);
        
        if (!_creatingTransactions.TryGetValue(_lastCreatingTransaction!, out var transaction))
            new InvalidOperationException("The transaction was not created").Log(this);
        
        if (!transaction.GetCommands().Any())
            new InvalidOperationException("The transaction has not any registered commands").Log(this);

        var command = transaction.GetCommands().Last();
    #if DEBUG
        Console.WriteLine($"scheduler: Executing {command.ToString()} in {transaction.ToString()}");
    #endif
        await command.ExecuteAsync();
        return command;
    }

    public virtual TransactionalCommandScheduler<T, TTransaction> CommitTransaction<TCurrentTransaction>() 
        where TCurrentTransaction : TTransaction
    {
        if (!CanContinueWorking) return this;
        
        if (!_creatingTransactions.TryGetValue(typeof(TCurrentTransaction), out var transaction))
            new InvalidOperationException("The transaction was not created or already commited").Log(this);

        if (!transaction.GetCommands().Any()) 
            new InvalidOperationException("The transaction has not any commands").Log(this);

        if (transaction is not T typedTransaction) 
            throw new InvalidOperationException("Transaction cannot be added to undo stack");
        
        _creatingTransactions.Remove(typeof(TCurrentTransaction));
        _undoStack.Push(typedTransaction);
        _redoStack.Clear();
    #if DEBUG
        Console.WriteLine($"scheduler: Commit {transaction.ToString()}");
    #endif
        return this;
    }

    public override async Task<object> UndoAsync() {
        if (!CanUndo()) 
            return default;
        
        var result = await base.UndoAsync();
        return result; // потом у команды можно взять тип
    }
}
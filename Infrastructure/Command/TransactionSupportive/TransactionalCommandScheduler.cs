using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public class TransactionalCommandScheduler<TCommand> : CommandScheduler<TCommand>
    where TCommand : ICommand
{
    protected CompositeCommandBase? _currentTransaction;
    public bool IsInTransaction => _currentTransaction != null;

    public override async Task ExecuteCommand(TCommand command) {
        if (_currentTransaction != null) {
            _currentTransaction.AddCommand(command);
            await command.ExecuteAsync();
        }
        else {
            await base.ExecuteCommand(command);
        }
    }
    
    public void BeginTransaction(string? transactionName = null) {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Transaction already started");
        
        _currentTransaction = new TransactionCommand(transactionName);
    }

    public bool CommitTransaction() {
        if (_currentTransaction == null)
            return false;

        var transaction = _currentTransaction;
        _currentTransaction = null;

        if (!transaction.GetCommands().Any()) return false;
        if (transaction is not TCommand typedTransaction) 
            throw new InvalidOperationException("Transaction cannot be added to undo stack");
        
        _undoStack.Push(typedTransaction);
        _redoStack.Clear();
        return true;
    }

    public void RollbackTransaction() {
        foreach (var command in _currentTransaction?.GetCommands().Reverse()!) 
            command.UndoAsync().Wait(); // В реальном коде лучше async/await
        
        _currentTransaction = null;
    }
}

public class TransactionCommand : CompositeCommandBase
{
    private readonly string? _name;
    public TransactionCommand(string? name) => _name = name;
}
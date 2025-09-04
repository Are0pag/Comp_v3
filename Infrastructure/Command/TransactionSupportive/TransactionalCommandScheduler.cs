using Infrastructure.Command.Base;

namespace Infrastructure.Command.TransactionSupportive;

public class TransactionalCommandScheduler<T, TTransaction> : CommandScheduler<T>
    where T : ICommand
    where TTransaction : ITransaction<T>, new()
{
    protected TTransaction? _currentTransaction;
    public bool IsInTransaction => _currentTransaction != null;

    public override async Task ExecuteCommand(T command) {
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

        _currentTransaction = new TTransaction();
    }

    public bool CommitTransaction() {
        if (_currentTransaction == null)
            return false;

        var transaction = _currentTransaction;
        _currentTransaction = default;

        if (!transaction.GetCommands().Any()) return false;
        if (transaction is not T typedTransaction) 
            throw new InvalidOperationException("Transaction cannot be added to undo stack");
        
        _undoStack.Push(typedTransaction);
        _redoStack.Clear();
        return true;
    }

    public void RollbackTransaction() {
        foreach (var command in _currentTransaction?.GetCommands().Reverse()!) 
            command.UndoAsync().Wait(); // В реальном коде лучше async/await
        
        _currentTransaction = default;
    }
}
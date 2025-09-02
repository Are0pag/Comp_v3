namespace Infrastructure.Command.Classic;

public interface ICommandScheduler<in T> where T : ICommand
{
    Task ExecuteCommand(T command);
    Task UndoAsync();
    Task RedoAsync();
}

public class HeterochromicCommandScheduler<TCommand> : CommandScheduler<TCommand> 
    where TCommand : ICommand, IDeferredCommand
{
    public async Task CommitDeferredChanges() {
        while (_undoStack.Count > 0) {
            var command = _undoStack.Pop();
            await command.ExecuteDeferredAsync();
        }
    }
}

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
        if (transaction is not TCommand typedTransaction) throw new InvalidOperationException("Transaction cannot be added to undo stack");
        _undoStack.Push(typedTransaction);
        _redoStack.Clear();
        return true;
    }

    public void RollbackTransaction() {
        // Отменяем все команды в транзакции
        foreach (var command in _currentTransaction?.GetCommands().Reverse()!) 
            command.UndoAsync().Wait(); // В реальном коде лучше async/await
        
        _currentTransaction = null;
    }

    private class TransactionCommand : CompositeCommandBase
    {
        private readonly string? _name;
        public TransactionCommand(string? name) => _name = name;
    }
}

/* Redo для транзакций не реализован */
public class CommandScheduler<TCommand> : ICommandScheduler<TCommand> 
    where TCommand : ICommand
{
    protected readonly Stack<TCommand> _undoStack = new();
    protected readonly Stack<TCommand> _redoStack = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public virtual async Task ExecuteCommand(TCommand command) {
        await command.ExecuteAsync();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public virtual async Task UndoAsync() {
        if (!CanUndo) return;
        var command = _undoStack.Pop();
        await command.UndoAsync();
        _redoStack.Push(command);
    }

    public virtual async Task RedoAsync() {
        if (!CanRedo) return;
        
        var command = _redoStack.Pop();
        await command.ExecuteAsync();
        _undoStack.Push(command);
    }
}
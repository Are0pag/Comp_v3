namespace Infrastructure.Command.Classic;

/* Redo для транзакций не реализован */
public class AdvancedUndoRedoManager
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();
    private CompositeCommandBase _currentTransaction;

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;
    
    public bool IsInTransaction => _currentTransaction != null;

    public event EventHandler<CommandExecutedEventArgs> CommandExecuted;

    public async Task ExecuteCommand(ICommand command) {
        if (_currentTransaction != null) {
            _currentTransaction.AddCommand(command);
            await command.ExecuteAsync().ConfigureAwait(false);
        }
        else {
            await command.ExecuteAsync().ConfigureAwait(false);
            _undoStack.Push(command);
            _redoStack.Clear();
            CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(command, CommandAction.Executed));
        }
    }

    public async Task Undo() {
        if (!CanUndo) return;
        var command = _undoStack.Pop();
        await command.UndoAsync().ConfigureAwait(false);
        _redoStack.Push(command);
        CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(command, CommandAction.Undone));
    }

    public async Task Redo() {
        if (!CanRedo) return;
        
        var command = _redoStack.Pop();
        await command.ExecuteAsync().ConfigureAwait(false);
        _undoStack.Push(command);
        
        CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(command, CommandAction.Redone));
    }

    public void BeginTransaction(string transactionName = null) {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Transaction already started");
        
        _currentTransaction = new TransactionCommand(transactionName);
    }

    public async Task<bool> CommitTransaction() {
        if (_currentTransaction == null)
            return false;

        var transaction = _currentTransaction;
        _currentTransaction = null;

        if (!transaction.GetCommands().Any()) return false;
        _undoStack.Push(transaction);
        _redoStack.Clear();
        CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(transaction, CommandAction.Executed));
        return true;
    }

    public void RollbackTransaction() {
        if (_currentTransaction == null) return;
        
        // Отменяем все команды в транзакции
        foreach (var command in _currentTransaction.GetCommands().Reverse()) 
            command.UndoAsync().Wait(); // В реальном коде лучше async/await
        
        _currentTransaction = null;
    }

    private class TransactionCommand : CompositeCommandBase
    {
        private readonly string _name;
        public TransactionCommand(string name) => _name = name;
    }
}
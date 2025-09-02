namespace Infrastructure.Command.Classic;

public interface ICommandSheduler<in T> where T : ICommand
{
    Task ExecuteCommand(T command);
    Task UndoAsync();
    Task RedoAsync();
}

public class HeterochCommandSheduler<TCommand> : AdvancedUndoRedoManager<TCommand> 
    where TCommand : ICommand, IDeferredCommand
{
    public override async Task ExecuteCommand(TCommand command) {
        await base.ExecuteCommand(command);
        await command.ExecuteDeferredAsync();
    }

    public Task UndoAsync() {
        throw new NotImplementedException();
    }

    public Task RedoAsync() {
        throw new NotImplementedException();
    }
}

/* Redo для транзакций не реализован */
public class AdvancedUndoRedoManager<TCommand> : ICommandSheduler<TCommand> 
    where TCommand : ICommand
{
    private readonly Stack<TCommand> _undoStack = new();
    private readonly Stack<TCommand> _redoStack = new();
    //private CompositeCommandBase _currentTransaction;

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;
    
    //public bool IsInTransaction => _currentTransaction != null;

    public event EventHandler<CommandExecutedEventArgs> CommandExecuted;

    public virtual async Task ExecuteCommand(TCommand command) {
        /*if (_currentTransaction != null) {
            _currentTransaction.AddCommand(command);
            await command.ExecuteAsync().ConfigureAwait(false);
        }
        else {*/
            await command.ExecuteAsync().ConfigureAwait(false);
            _undoStack.Push(command);
            _redoStack.Clear();
            CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(command, CommandAction.Executed));
        /*}*/
    }

    public virtual async Task UndoAsync() {
        if (!CanUndo) return;
        var command = _undoStack.Pop();
        await command.UndoAsync().ConfigureAwait(false);
        _redoStack.Push(command);
        CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(command, CommandAction.Undone));
    }

    public virtual async Task RedoAsync() {
        if (!CanRedo) return;
        
        var command = _redoStack.Pop();
        await command.ExecuteAsync().ConfigureAwait(false);
        _undoStack.Push(command);
        
        CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(command, CommandAction.Redone));
    }

    /*public void BeginTransaction(string transactionName = null) {
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
    }*/
}
using Infrastructure.Command.Base;
using Infrastructure.Command.TransactionSupportive;

namespace Infrastructure.Command.Heterochromic;

public class HeterochromicCommandScheduler<T, TTransaction> : TransactionalCommandScheduler<T, TTransaction>, IHeterochromicCommandScheduler<T, TTransaction>
    where T : IDeferredCommand
    where TTransaction : ITransaction<T>, new()
{
    public virtual async Task CommitDeferredChanges() {
        while (_undoStack.Count > 0) {
            var command = _undoStack.Pop();
            await command.ExecuteDeferredAsync();
        }
    }

    /// <summary>
    /// То есть добавить в график команду, которая уже была выполнена
    /// </summary>
    public virtual void PushDeferredCommand(T command) {
        _undoStack.Push(command);
        _redoStack.Clear();
        OnCommandExecuted?.Invoke(CommandAction.Executed, command);
    }
}
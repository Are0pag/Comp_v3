using Infrastructure.Command.Base;
using Infrastructure.Command.TransactionSupportive;

namespace Infrastructure.Command.Heterochromic;

public class HeterochromicCommandScheduler<TCommand> : TransactionalCommandScheduler<TCommand> 
    where TCommand : ICommand, IDeferredCommand
{
    public async Task CommitDeferredChanges() {
        while (_undoStack.Count > 0) {
            var command = _undoStack.Pop();
            await command.ExecuteDeferredAsync();
        }
    }
}


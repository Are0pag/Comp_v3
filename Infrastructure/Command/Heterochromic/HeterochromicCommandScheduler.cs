namespace Infrastructure.Command.Classic;

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
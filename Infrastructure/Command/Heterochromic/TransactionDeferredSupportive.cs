using Infrastructure.Command.TransactionSupportive;

namespace Infrastructure.Command.Heterochromic;

public class TransactionDeferredSupportive : TransactionBase<IDeferredCommand>, IDeferredCommand
{
    public async Task ExecuteDeferredAsync() {
        for (var i = _commands.Count - 1; i >= 0; i--) {
            var command = _commands[i];
            await command.ExecuteDeferredAsync();
        }
    }

    public override string ToString() {
        return GetType().Name;
    }
}
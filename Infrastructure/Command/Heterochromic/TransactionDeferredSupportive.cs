using Infrastructure.Command.TransactionSupportive;

namespace Infrastructure.Command.Heterochromic;

public class TransactionDeferredSupportive : TransactionBase<IDeferredCommand>, IDeferredCommand
{
    public async Task ExecuteDeferredAsync() {
        foreach (var deferred in _commands)
            await deferred.ExecuteDeferredAsync();
    }

    public override string ToString() {
        return GetType().Name;
    }
}
namespace Infrastructure.Command.Heterochromic;

public class DeferredCommand<TContext, TItem> : IDeferredCommand
{
    protected readonly TContext _context;
    protected TItem? _item;

    public DeferredCommand(TContext context) {
        _context = context;
    }

    public async virtual Task ExecuteAsync() {
        throw new NotImplementedException();
    }

    public async virtual Task UndoAsync() {
        throw new NotImplementedException();
    }

    public async virtual Task ExecuteDeferredAsync() {
        throw new NotImplementedException();
    }
}
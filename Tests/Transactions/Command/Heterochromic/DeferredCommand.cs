namespace Infrastructure.Command.Heterochromic;

public abstract class DeferredCommand<TContext, TItem> : IDeferredCommand
{
    protected readonly TContext _context;
    protected TItem? _item;

    public DeferredCommand(TContext context) {
        _context = context;
    }

    public abstract Task ExecuteAsync();
    public abstract Task UndoAsync();
    public abstract Task ExecuteDeferredAsync();
}
using Infrastructure.Command.Base;

namespace Infrastructure.Command.Heterochromic;

public abstract class DeferredCommand<TContext, TItem> : IDeferredCommand
{
    protected readonly TContext _context;
    protected TItem? _item;

    public DeferredCommand(TContext context) {
        _context = context;
    }

    public string? Description { get; set; }

    public abstract Task ExecuteAsync();
    public abstract Task UndoAsync();

    public bool IsExecuted { get; protected set; }

    public ICommand MarkAs(bool isExecuted) {
        IsExecuted = isExecuted;
        return this;
    }

    public abstract Task ExecuteDeferredAsync();
}
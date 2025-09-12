namespace Infrastructure.Command.Heterochromic;

public abstract class DeferredCommand<TItem> : IDeferredCommand
{
    protected TItem? _item;
    public string? Description { get; set; }

    public abstract Task ExecuteAsync();
    public abstract Task UndoAsync();
    public abstract Task ExecuteDeferredAsync();
}
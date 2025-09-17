namespace Infrastructure.Command.Heterochromic;

public abstract class DeferredCommand<TItem> : IDeferredCommand
{
    public string? Description { get; set; }

    public abstract Task ExecuteAsync();
    public abstract Task UndoAsync();
    public abstract Task ExecuteDeferredAsync();
}
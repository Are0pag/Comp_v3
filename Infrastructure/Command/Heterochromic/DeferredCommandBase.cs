namespace Infrastructure.Command.Heterochromic;

public abstract class DeferredCommandBase<TParameter> : IDeferredCommand
{
    protected readonly TParameter _parameter;
    protected DeferredCommandBase(TParameter parameter) {
        _parameter = parameter;
    }
    
    public string? Description { get; set; }

    public virtual Task ExecuteAsync() => Task.CompletedTask;
    public virtual Task UndoAsync() => Task.CompletedTask;
    public virtual Task ExecuteDeferredAsync() => Task.CompletedTask;
}
namespace Infrastructure.Command.Classic;

public interface ICommand
{
    public abstract Task ExecuteAsync();
    public abstract Task UndoAsync();
}

public interface IDeferredCommand : ICommand
{
    Task ExecuteDeferredAsync();
}
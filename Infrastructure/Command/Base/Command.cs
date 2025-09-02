namespace Infrastructure.Command.Classic;

public interface ICommand
{
    Task ExecuteAsync();
    Task UndoAsync();
}

public interface IDeferredCommand : ICommand
{
    Task ExecuteDeferredAsync();
}
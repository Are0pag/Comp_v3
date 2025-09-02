namespace Infrastructure.Command.Classic;

public interface ICommand
{
    Task ExecuteAsync();
    Task UndoAsync();
}
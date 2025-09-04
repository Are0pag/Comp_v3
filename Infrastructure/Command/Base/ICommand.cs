namespace Infrastructure.Command.Base;

public interface ICommand
{
    Task ExecuteAsync();
    Task UndoAsync();
}
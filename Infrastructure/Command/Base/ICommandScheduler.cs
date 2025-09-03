namespace Infrastructure.Command.Base;

public interface ICommandScheduler<in T> where T : ICommand
{
    Task ExecuteCommand(T command);
    Task UndoAsync();
    Task RedoAsync();
}
namespace Infrastructure.Command.Classic;

public interface ICommandScheduler<in T> where T : ICommand
{
    Task ExecuteCommand(T command);
    Task UndoAsync();
    Task RedoAsync();
}
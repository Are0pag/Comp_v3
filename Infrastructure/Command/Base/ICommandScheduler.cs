namespace Infrastructure.Command.Base;

public interface ICommandScheduler<T> where T : ICommand
{
    bool CanUndo();
    bool CanRedo();
    Task ExecuteCommand(T command);
    Task UndoAsync();
    Task RedoAsync();
}
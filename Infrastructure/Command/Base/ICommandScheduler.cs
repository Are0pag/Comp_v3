namespace Infrastructure.Command.Base;

public interface ICommandScheduler<T> where T : ICommand
{
    bool CanUndo();
    bool CanRedo();
    Task<ICommandScheduler<T>> ExecuteCommand(T command);
    Task<object> UndoAsync();
    Task<object> RedoAsync();
}
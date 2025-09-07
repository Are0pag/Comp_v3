namespace Infrastructure.Command.Base;

public interface ICommand
{
    string? Description { get; set; }
    Task ExecuteAsync();
    Task UndoAsync();
    
    bool IsExecuted { get; }
    ICommand MarkAs(bool isExecuted);
}

public enum CommandStatus
{
    Completed,
    Planned
}
namespace Infrastructure.Command.Base;

public interface ICommand
{
    string? Description { get; set; }
    Task ExecuteAsync();
    Task UndoAsync();
}
namespace Infrastructure.Command.Classic;

public interface IDeferredCommand : ICommand
{
    Task ExecuteDeferredAsync();
}
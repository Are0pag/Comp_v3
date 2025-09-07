using Infrastructure.Command.Base;

namespace Infrastructure.Command.Heterochromic;

public interface IDeferredCommand : ICommand
{
    Task ExecuteDeferredAsync();
}
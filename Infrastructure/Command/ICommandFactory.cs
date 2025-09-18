using Infrastructure.Command.Heterochromic;

namespace Infrastructure.Command;

public interface ICommandFactory
{
    TCommand CreateCommand<TCommand, TParameter>(TParameter parameter)
        where TCommand : DeferredCommandBase<TParameter>;
}
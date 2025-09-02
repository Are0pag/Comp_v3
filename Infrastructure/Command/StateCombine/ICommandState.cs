using Infrastructure.Command.Classic;
using Infrastructure.StateMachine;

namespace Infrastructure.Command.StateCombine;

public interface ICommandState<TContext> : IState<TContext> where TContext : class
{
    Task<ICommand> GetEnterCommand(TContext context);
    Task<ICommand> GetExitCommand(TContext context);
}
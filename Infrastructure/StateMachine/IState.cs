namespace Infrastructure.StateMachine;

public interface IState<TContext> where TContext : class
{
    Task Enter(TContext context);
    Task Exit(TContext context);
}
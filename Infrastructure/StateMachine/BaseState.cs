namespace Infrastructure.StateMachine;

public abstract class BaseState<TContext> : IState<TContext> where TContext : class
{
    public virtual Task Enter(TContext context) => Task.CompletedTask;
    public virtual Task Exit(TContext context) => Task.CompletedTask;
}
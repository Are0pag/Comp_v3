namespace Infrastructure.StateMachine;

public interface IStateMachine<TState, TContext> 
    where TState : IState<TContext>
    where TContext : class
{
    TState CurrentState { get; }
    Task ChangeState(TState newState);
    TState GetState<T>() where T : TState;
}
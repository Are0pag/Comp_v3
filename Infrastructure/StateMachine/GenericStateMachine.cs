namespace Infrastructure.StateMachine;

public class GenericStateMachine<TState, TContext> : IStateMachine<TState, TContext>
    where TState : class, IState<TContext>
    where TContext : class
{
    private readonly Dictionary<Type, TState> _states;

    protected GenericStateMachine(IEnumerable<TState> states, TState initialState) {
        _states = states.ToDictionary(state => state.GetType());
        CurrentState = initialState;
    }
    public TState CurrentState { get; protected set; }

    public virtual async Task ChangeState(TState newState, TContext context) {
        await CurrentState.Exit(context).ConfigureAwait(false);
        CurrentState = newState;
        await CurrentState.Enter(context);
    }

    /// <summary>
    /// В разработке. Требует тщательного анализа
    /// </summary>
    public virtual async Task RollbackState(TState newState, TContext context) {
        CurrentState = newState;
    }

    public TState GetState<T>() where T : TState => _states[typeof(T)];
}
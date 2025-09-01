namespace Infrastructure.StateMachine;

public class GenericStateMachine<TState, TContext> : IStateMachine<TState, TContext>
    where TState : class, IState<TContext>
    where TContext : class
{
    private readonly Dictionary<Type, TState> _states;

    protected GenericStateMachine(IEnumerable<TState> states, TContext context, TState initialState) {
        _states = states.ToDictionary(state => state.GetType());
        Context = context;
        CurrentState = initialState;
        CurrentState.Enter(Context);
    }
    public TState CurrentState { get; protected set; }
    public TContext Context { get; set; }

    public virtual async Task ChangeState(TState newState) {
        await CurrentState.Exit(Context).ConfigureAwait(false);
        CurrentState = newState;
        await CurrentState.Enter(Context);
    }

    public TState GetState<T>() where T : TState => _states[typeof(T)];
}
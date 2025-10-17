using System.Windows.Input;
using Comp_v4.NomDict.Events;
using Comp.ModelData.Comp;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.NomDict.Entities;

public class Grid : GenericStateMachine<BaseSGridState, Grid>, IGridSelectingStateHandler
{
    public Grid(IEnumerable<BaseSGridState> states, BaseSGridState initialState) : base(states, initialState) {
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }

    public virtual void OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        CurrentState.OnMouseDoubleClick(sender, e, this);
    }

    public virtual void AddComponent(object? parameter) {
        CurrentState.AddComponent(parameter);
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }

    public void OnSelecting(TaskCompletionSource<Component> tcs) {
        _ = ChangeState(GetState<SelectionGridState>(), this);
    }
}
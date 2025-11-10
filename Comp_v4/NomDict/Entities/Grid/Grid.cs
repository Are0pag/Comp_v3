using System.Windows.Input;
using Comp_v4.NomDict.Events;
using Comp.ModelData.Comp;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.NomDict.Entities;

public class Grid : GenericStateMachine<BaseSGridState, Grid>//, IGridSelectingStateHandler
{
    public Grid(IEnumerable<BaseSGridState> states, BaseSGridState initialState) : base(states, initialState) {
        //EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }

    public async Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs e) {
        await CurrentState.OnMouseDoubleClick(tcs, sender, e, this);
    }

    public async Task AddComponent(TaskCompletionSource tcs, object? parameter) {
        await CurrentState.Add(tcs, parameter, this);
    }

    /*public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }*/

    public void OnSelecting(TaskCompletionSource<Component> tcs) {
        _ = ChangeState(GetState<SelectionGridState>(), this);
    }
}
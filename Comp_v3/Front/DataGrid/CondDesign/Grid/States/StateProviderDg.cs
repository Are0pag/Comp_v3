using Comp_v3.Front.Events.Buttons;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateProviderDg : GenericStateMachine<StateDataGrid, CognDesignGridVm>
{
    public StateProviderDg(IEnumerable<StateDataGrid> states, StateDataGrid initialState) 
        : base(states,  initialState) {
    }

    public override Task ChangeState(StateDataGrid newState, CognDesignGridVm context) {
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
        return base.ChangeState(newState, context);
    }
}
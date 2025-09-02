using Infrastructure.StateMachine;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateProviderDg : GenericStateMachine<StateDataGrid, CognDesignGridVm>
{
    public StateProviderDg(IEnumerable<StateDataGrid> states, StateDataGrid initialState) 
        : base(states,  initialState) {
    }
}
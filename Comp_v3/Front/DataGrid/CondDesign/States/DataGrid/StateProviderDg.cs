using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Infrastructure.StateMachine;

namespace Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;

public class StateProviderDg : GenericStateMachine<StateDataGrid, CognDesignGridVm>
{
    public StateProviderDg(IEnumerable<StateDataGrid> states, StateDataGrid initialState) 
        : base(states,  initialState) {
    }
}
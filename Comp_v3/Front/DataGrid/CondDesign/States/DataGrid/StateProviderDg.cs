namespace Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;

public class StateProviderDg
{
    public StateProviderDg(StateDgEditing stateEditing, StateDgCreatingNewItem createNewItem) {
        Editing = stateEditing;
        CreateNewItem = createNewItem;
        CurrentStateDataGrid = GetInitialState();
    }
    
    public StateDataGrid CurrentStateDataGrid { get; protected set; }
    public StateDgEditing Editing { get; } 
    public StateDgCreatingNewItem CreateNewItem { get;  }

    public StateDataGrid GetInitialState() => Editing;

    public void ChangeState(StateDataGrid stateDataGrid) {
        CurrentStateDataGrid = stateDataGrid;
    }
}
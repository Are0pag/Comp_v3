namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public class Provider
{
    public Provider(StateWaitingToInputIntoNewItem stateWaitingToInputIntoNewItem, StateEditableGrid stateEditableGrid) {
        StateWaitingToInputIntoNewItem = stateWaitingToInputIntoNewItem;
        StateEditableGrid = stateEditableGrid;
        CurrentStateWindow = StateEditableGrid; 
    }

    public StateWindow CurrentStateWindow { get; protected set; }
    public StateWaitingToInputIntoNewItem StateWaitingToInputIntoNewItem { get; }
    public StateEditableGrid StateEditableGrid { get; }

    public void SwitchStateWindow(StateWindow stateWindow, CognDesignGridWindow window) {
        CurrentStateWindow.Exit(window);
        CurrentStateWindow = stateWindow;
        CurrentStateWindow.Entry(window);
    }

    public void RollbackStateWindow(StateWindow stateWindow, CognDesignGridWindow window) {
        CurrentStateWindow = stateWindow;
    }
}
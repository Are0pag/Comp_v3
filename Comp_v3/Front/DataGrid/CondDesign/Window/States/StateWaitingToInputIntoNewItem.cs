using System.Windows.Controls;
using Comp_v3.Front.Events;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public class StateWaitingToInputIntoNewItem : StateWindow
{
    public override void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        try {
            base.OnCellEditEnding(window, sender, e);
        }
        catch (InvalidInputException ex) { 
            return;
        }
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellAddingToDataGridHandler>(h => h.HandleNewValueAdding());
        window.StateProvider.SwitchStateWindow(window.StateProvider.StateEditableGrid, window);
    }
}
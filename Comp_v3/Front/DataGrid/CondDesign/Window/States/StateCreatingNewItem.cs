using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

/// <summary>
/// То есть состояние UI когда создаётся новый пустой объект
/// </summary>
public class StateCreatingNewItem : StateWindow
{
    public override void OneNewValueAdded(CognDesignGridWindow window, object newValue) {
        if (newValue is not ConditionalDesignation conditionalDesignation) 
            throw new ArgumentException("New value is not a conditional designation in CognDesignGridWindow");
        
        window.InfoDataGrid.Focus();
        window.InfoDataGrid.ScrollIntoView(conditionalDesignation);
        window.InfoDataGrid.SelectedItem = conditionalDesignation;

        Dispatcher.CurrentDispatcher.BeginInvoke(() => {
            ManageCursorPositionByColumnsTypes(window, conditionalDesignation);
        }, DispatcherPriority.ContextIdle);
        
        window.StateProvider.SwitchStateWindow(window.StateProvider.StateWaitingToInputIntoNewItem, window);
    }
    
    protected virtual void ManageCursorPositionByColumnsTypes(CognDesignGridWindow window, ConditionalDesignation conditionalDesignation) {
        if (window.InfoDataGrid.ItemContainerGenerator.ContainerFromItem(conditionalDesignation) is not DataGridRow row) 
            throw new ArgumentException("CognDesignGridWindow could not find raw Row");
        
        // Находим первую редактируемую колонку
        var editableColumn = window.InfoDataGrid.Columns
                                   .FirstOrDefault(column => !column.IsReadOnly && column.Visibility == Visibility.Visible);
        if (editableColumn != null) {
            window.InfoDataGrid.CurrentCell = new DataGridCellInfo(conditionalDesignation, editableColumn);
            window.InfoDataGrid.BeginEdit();
            return;
        }
        row.Focus();
    }
}

public class StateWaitingToInputIntoNewItem : StateWindow
{
    public override void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellAddingToDataGridHandler>(h => h.HandleNewValueAdding());
        window.StateProvider.SwitchStateWindow(window.StateProvider.StateEditableGrid, window);
    }
}

public class StateEditableGrid : StateWindow
{
    public override void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellEditEndingHandler>(h => h.HandleCellEdit(sender, e));
    }
}
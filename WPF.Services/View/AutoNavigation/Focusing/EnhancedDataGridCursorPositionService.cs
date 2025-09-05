using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public class EnhancedDataGridCursorPositionService : CursorPositionService<DataGrid>
{
    public void MoveCursorToLeftCell(DataGrid dataGrid) => new Mover().MoveToLeftCell(dataGrid);
    public override EnhancedDataGridMemento FocusAndEditItem(DataGrid dataGrid, object item, Dispatcher dispatcher) {
        var memento = new EnhancedDataGridMemento(dataGrid);
        
        dataGrid.Focus();
        dataGrid.ScrollIntoView(item);
        dataGrid.SelectedItem = item;
        
        dispatcher.BeginInvoke(() => {
            ManageCursorPosition(dataGrid, item);
        }, DispatcherPriority.ContextIdle);
        
        return memento;
    }

    public override EnhancedDataGridMemento FocusItem(DataGrid dataGrid, object item) {
        var memento = new EnhancedDataGridMemento(dataGrid);
        
        dataGrid.Focus();
        dataGrid.ScrollIntoView(item);
        dataGrid.SelectedItem = item;
        
        return memento;
    }

    public override EnhancedDataGridMemento RememberCursorPos(DataGrid container) => new(container);

    protected void ManageCursorPosition(DataGrid dataGrid, object item) {
        if (dataGrid.ItemContainerGenerator.ContainerFromItem(item) is not DataGridRow row)
            return;

        var editableColumn = dataGrid.Columns
                                     .FirstOrDefault(column => !column.IsReadOnly && column.Visibility == Visibility.Visible);
            
        if (editableColumn != null) {
            dataGrid.CurrentCell = new DataGridCellInfo(item, editableColumn);
            dataGrid.BeginEdit();
        }
        else {
            row.Focus();
        }
    }

    public EnhancedDataGridMemento PreventAutoNavigation(DataGrid dataGrid, Action action) {
        var memento = RememberCursorPos(dataGrid);
        
        try {
            action();
        }
        finally {
            memento.Restore(dataGrid);
        }
        
        return memento;
    }
}
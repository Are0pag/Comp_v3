using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public class DataGridCursorPositionService : CursorPositionService<DataGrid>
{
    public override DataGridMemento FocusAndEditItem(DataGrid dataGrid, object item, Dispatcher dispatcher) {
        var memento = new DataGridMemento(dataGrid);
        
        dataGrid.Focus();
        dataGrid.ScrollIntoView(item);
        dataGrid.SelectedItem = item;
        
        dispatcher.BeginInvoke(() => {
            ManageCursorPosition(dataGrid, item);
        }, DispatcherPriority.ContextIdle);
        return memento;
    }

    /// <summary>
    /// WithoutEditing
    /// </summary>
    public override DataGridMemento FocusItem(DataGrid dataGrid, object item) {
        var memento = new DataGridMemento(dataGrid);
        
        dataGrid.Focus();
        dataGrid.ScrollIntoView(item);
        dataGrid.SelectedItem = item;
        return memento;
    }

    public override DataGridMemento RememberCursorPos(DataGrid container) => new(container);

    protected void ManageCursorPosition(DataGrid dataGrid, object item) {
        if (dataGrid.ItemContainerGenerator.ContainerFromItem(item) is not DataGridRow row)
            throw new ArgumentException("Could not find DataGridRow for item");

        var editableColumn = dataGrid.Columns
                                     .FirstOrDefault(column => !column.IsReadOnly && column.Visibility == Visibility.Visible);
            
        if (editableColumn != null) {
            dataGrid.CurrentCell = new DataGridCellInfo(item, editableColumn);
            dataGrid.BeginEdit();
            return;
        }
            
        row.Focus();
    }
}
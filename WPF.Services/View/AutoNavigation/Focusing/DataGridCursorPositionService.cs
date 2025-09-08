using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public class DataGridCursorPositionService : CursorPositionService<DataGrid>
{
    public override DataGridMemento RememberCursorPos(DataGrid container) => new(container);

    public override DataGridMemento FocusAndEditFirstEditableItem(DataGrid dataGrid, object item) {
        var memento = new DataGridMemento(dataGrid);
        
        dataGrid.Focus();
        dataGrid.ScrollIntoView(item);
        dataGrid.SelectedItem = item;
        var firstEditableColumn = dataGrid.Columns
                                          .FirstOrDefault(column => !column.IsReadOnly && column.Visibility == Visibility.Visible);
        
        Dispatcher.CurrentDispatcher.BeginInvoke(() => {

            if (firstEditableColumn == null)
                throw new InvalidOperationException("DataGrid have not any editable items");
            
            ManageCursorPosition(dataGrid, item, firstEditableColumn);
        }, DispatcherPriority.ContextIdle);
        return memento;
    }

    public override CellMemento FocusAndEditItem(DataGrid dataGrid, DataGridCellEditEndingEventArgs e) {
        return new CellMemento(dataGrid, e).TryFocusLastEditedCell();
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

    protected void ManageCursorPosition(DataGrid dataGrid, object item, DataGridColumn column) {
        if (dataGrid.ItemContainerGenerator.ContainerFromItem(item) is not DataGridRow row)
            throw new ArgumentException("Could not find DataGridRow for item");
            
        if (column == null) return;
        
        dataGrid.CurrentCell = new DataGridCellInfo(item, column);
        dataGrid.BeginEdit();
        row.Focus();
    }
}
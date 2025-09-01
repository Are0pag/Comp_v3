using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public class DataGridCursorPositionService : CursorPositionService<DataGrid>
{
    public override void FocusAndEditItem(DataGrid dataGrid, object item, Dispatcher dispatcher) {
        FocusItem(dataGrid, item);
        dispatcher.BeginInvoke(() => {
            ManageCursorPosition(dataGrid, item);
        }, DispatcherPriority.ContextIdle);
    }

    /// <summary>
    /// WithoutEditing
    /// </summary>
    public override void FocusItem(DataGrid dataGrid, object item) {
        dataGrid.Focus();
        dataGrid.ScrollIntoView(item);
        dataGrid.SelectedItem = item;
    }

    // Дополнительные специфичные методы
    public void FocusAndEditSpecificColumn(DataGrid dataGrid, object item, string columnName) {
        var column = dataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
        if (column == null) return;
        dataGrid.CurrentCell = new DataGridCellInfo(item, column);
        dataGrid.BeginEdit();
    }

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
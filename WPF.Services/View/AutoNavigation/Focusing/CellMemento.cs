using System.Windows.Controls;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public class CellMemento
{
    public DataGrid DataGrid { get; }
    public DataGridCellInfo EditedCell { get; }
    public object EditedCellContent { get; }

    public CellMemento(DataGrid dataGrid, DataGridCellEditEndingEventArgs e) {
        EditedCellContent = e.Row.Item;
        EditedCell = new DataGridCellInfo(EditedCellContent, e.Column);
        DataGrid = dataGrid;
    }

    public CellMemento TryFocusLastEditedCell() {
        try {
            FocusLastEditedCellRecursive();
        }
        catch {
            throw new StackOverflowException("FocusLastEditedCellRecursive executed with stack overflow.");
        }
        return this;
    }

    protected void FocusLastEditedCellRecursive() {
        if (EditedCell is not { IsValid: true }) 
            return;
    
        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
            if (DataGrid.ItemContainerGenerator.ContainerFromItem(EditedCell.Item) is not DataGridRow row) {
                // Если строка не видна, прокручиваем к ней
                DataGrid.ScrollIntoView(EditedCell.Item);
            
                // И пытаемся сфокусироваться после прокрутки
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
                    FocusLastEditedCellRecursive();
                }), DispatcherPriority.Loaded);
            }
            else {
                var cell = GetCell(row, EditedCell.Column);
                if (cell == null) return;
                cell.Focus();
                DataGrid.CurrentCell = EditedCell;
                DataGrid.SelectedItem = EditedCellContent;
                DataGrid.BeginEdit();
            }
        }), DispatcherPriority.Background);
    }

    protected DataGridCell GetCell(DataGridRow row, DataGridColumn column) {
        var cellContent = column.GetCellContent(row);
        return cellContent?.Parent as DataGridCell;
    }
}
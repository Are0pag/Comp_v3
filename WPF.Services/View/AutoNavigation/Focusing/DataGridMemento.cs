using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.Services.View.AutoNavigation.Focusing;

public class DataGridMemento
{
    public object? PreviousSelectedItem { get; }
    public DataGridCellInfo PreviousCurrentCell { get; }
    public bool WasFocused { get; }

    public DataGridMemento(DataGrid dataGrid) {
        PreviousSelectedItem = dataGrid.SelectedItem;
        PreviousCurrentCell = dataGrid.CurrentCell;
        WasFocused = dataGrid.IsFocused;
    }

    public virtual void Restore(DataGrid dataGrid) {
        dataGrid.SelectedItem = PreviousSelectedItem;
        dataGrid.CurrentCell = PreviousCurrentCell;
        if (WasFocused) 
            dataGrid.Focus();
    }
}
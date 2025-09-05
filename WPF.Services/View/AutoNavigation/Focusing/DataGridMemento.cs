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


public class EnhancedDataGridMemento : DataGridMemento
{
    private readonly DataGrid _dataGrid;
    public EnhancedDataGridMemento(DataGrid dataGrid) : base(dataGrid) {
        _dataGrid = dataGrid;
        PreviousCurrentColumnIndex = dataGrid.CurrentColumn?.DisplayIndex;
        PreviousCurrentRowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
        
        var scrollViewer = FindScrollViewerRecursive(dataGrid);
        if (scrollViewer != null) ScrollState = new ScrollViewerState(scrollViewer);
    }
    public int? PreviousCurrentColumnIndex { get; protected set; }
    public int? PreviousCurrentRowIndex { get; protected set; }
    public bool WasEditing { get; protected set; }
    public ScrollViewerState? ScrollState { get; protected set; }

    public override void Restore(DataGrid dataGrid) {
        // Восстанавливаем выделение
        if (PreviousSelectedItem != null && dataGrid.Items.Contains(PreviousSelectedItem)) 
            dataGrid.SelectedItem = PreviousSelectedItem;

        // Восстанавливаем ячейку
        if (PreviousCurrentCell.IsValid && dataGrid.Items.Contains(PreviousCurrentCell.Item)) 
            dataGrid.CurrentCell = PreviousCurrentCell;

        // Восстанавливаем фокус
        if (WasFocused) dataGrid.Focus();

        // Восстанавливаем прокрутку
        if (ScrollState == null) return;
        var scrollViewer = FindScrollViewerRecursive(dataGrid);
        scrollViewer?.ScrollToHorizontalOffset(ScrollState.HorizontalOffset);
        scrollViewer?.ScrollToVerticalOffset(ScrollState.VerticalOffset);
    }
    private ScrollViewer? FindScrollViewerRecursive(DependencyObject parent) {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is ScrollViewer scrollViewer)
                return scrollViewer;
            
            var result = FindScrollViewerRecursive(child);
            if (result != null)
                return result;
        }
        return null;
    }
}

public class ScrollViewerState
{
    public double HorizontalOffset { get; set; }
    public double VerticalOffset { get; set; }
    
    public ScrollViewerState(ScrollViewer scrollViewer) {
        HorizontalOffset = scrollViewer.HorizontalOffset;
        VerticalOffset = scrollViewer.VerticalOffset;
    }
}
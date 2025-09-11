using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPF.Extensions.View.Elements;

public static class DataGridExtensions
{
    public static DataGridColumn GetFirstEditableColumn(this DataGrid dataGrid) {
        if (dataGrid.Columns.FirstOrDefault(column => !column.IsReadOnly && column.Visibility == Visibility.Visible) is not { } column)
            throw new ArgumentException($"First editable column not found");
        return column;
    }
    
    /// <summary>
    /// Получает строку DataGridRow для указанного элемента данных. Необходимо обернуть в Dispatcher.CurrentDispatcher.BeginInvoke
    /// </summary>
    public static DataGridRow GetRowFromItem(this DataGrid dataGrid, object item) {
        return dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
    }

    /// <summary>
    /// Получает ячейку в указанном столбце для заданной строки. Необходимо обернуть в Dispatcher.CurrentDispatcher.BeginInvoke
    /// </summary>
    public static DataGridCell GetCellInColumn(this DataGridRow row, DataGridColumn column) {
        var presenter = row.GetVisualChildRecursive<DataGridCellsPresenter>();
        return presenter?.ItemContainerGenerator.ContainerFromIndex(column.DisplayIndex) as DataGridCell;
    }

    /// <summary>
    /// Находит ячейку для конкретного элемента данных и столбца. Необходимо обернуть в Dispatcher.CurrentDispatcher.BeginInvoke
    /// </summary>
    public static DataGridCell GetCell(this DataGrid dataGrid, DataGridRow row, DataGridColumn column) {
        return row.GetCellInColumn(column);
    }

    /// <summary>
    /// Вспомогательный метод для получения дочернего визуального элемента
    /// </summary>
    private static T GetVisualChildRecursive<T>(this DependencyObject parent) 
        where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);
            if (child is T typedChild)
                return typedChild;

            var recursiveChild = child.GetVisualChildRecursive<T>();
            if (recursiveChild != null)
                return recursiveChild;
        }
        return null;
    }
}
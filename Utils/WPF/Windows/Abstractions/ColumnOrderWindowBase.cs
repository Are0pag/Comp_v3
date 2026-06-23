using System.Windows;
using System.Windows.Controls;

namespace Utils.WPF.Windows;

public abstract class ColumnOrderWindowBase : PosWindowBase
{
    protected DataGrid? _dg;
    public ColumnOrderWindowBase() {
        Loaded += ColumnOrderWindowBase_Loaded;
        Unloaded += ColumnOrderWindowBase_Unloaded;
    }

    private void ColumnOrderWindowBase_Loaded(object sender, RoutedEventArgs e) {
        Loaded -= ColumnOrderWindowBase_Loaded;
        _dg ??= GetDataGrid();
        ColumnOrderSettings.LoadColumnsOrder(_dg, GetType().ToString());
        _dg.ColumnReordered += DataGrid_ColumnReordered;
    }

    protected DataGrid? GetDataGrid() {
        if (VisualHelper.FindVisualChild<DataGrid>(this) is { } dg)
            return dg;
        Console.Error.WriteLine("No DataGrid found");
        throw new Exception("No DataGrid found");
    }
    
    private void DataGrid_ColumnReordered(object? sender, DataGridColumnEventArgs e) {
        if (_dg != null) {
            // Сохраняем новый порядок сразу при перетаскивании колонки
            ColumnOrderSettings.SaveColumnsOrder(_dg, GetType().ToString());
        }
    }

    private void ColumnOrderWindowBase_Unloaded(object sender, RoutedEventArgs e) {
        Unloaded -= ColumnOrderWindowBase_Unloaded;
        if (_dg != null) {
            _dg.ColumnReordered -= DataGrid_ColumnReordered;
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using WPF.Extensions.View.Elements;

namespace Utils.WPF.Windows;

public abstract class ColumnsVisibilityTableWindowBase : ColumnOrderWindowBase
{
    protected abstract ComboBox ColumnsVisibilityComboBox { get; }

    public ColumnsVisibilityTableWindowBase() {
        Loaded += TableWindowBase_Loaded;
    }
    
    protected void TableWindowBase_Loaded(object sender, RoutedEventArgs e) {
        Loaded -= TableWindowBase_Loaded;

        // Автоматически находим оба компонента на форме дочернего окна
        //var comboBox = FindName("ColumnsVisibilityComboBox") as ComboBox;

        // Если компоненты найдены, восстанавливаем состояние колонок
        _dg ??= GetDataGrid();
        ColumnVisibilitySettings.LoadColumnsVisibility(_dg, GetType().ToString(), ColumnsVisibilityComboBox);
    }

    protected void CheckBox_Click(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.Tag is string columnName) {
            var dataGrid = VisualHelper.FindVisualChild<DataGrid>(this);
            var column = dataGrid.FindName(columnName) as DataGridColumn;
            if (column != null) {
                column.Visibility = checkBox.IsChecked == true
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                // СРАЗУ СОХРАНЯЕМ ИЗМЕНЕНИЯ
                // "MainTableColumns" — это уникальный ключ для этой таблицы в json-файле
                ColumnVisibilitySettings.SaveColumnsVisibility(dataGrid, GetType().ToString());
            }
        }
    }
}

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

public abstract class TableWindowBase : PosWindowBase
{
    public TableWindowBase() {
        var dataGrid = VisualHelper.FindVisualChild<DataGrid>(this);
        
        Closed += (sender, args) => {
            try {
                // Находим DataGrid на форме через логическое дерево
                
                if (dataGrid != null && dataGrid.IsEditing()) {
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            
            this.Dispose();
        };
    }
}

public class WindowBase : Window
{

}
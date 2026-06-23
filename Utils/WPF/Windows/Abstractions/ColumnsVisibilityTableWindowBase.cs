using System.Windows;
using System.Windows.Controls;

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
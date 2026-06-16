using System.Windows;
using System.Windows.Controls;
using WPF.Extensions.View.Elements;

namespace Utils.WPF.Windows;

public abstract class ColumnsVisibilityTableWindowBase : PosWindowBase
{
    protected abstract ComboBox ColumnsVisibilityComboBox { get; }

    public ColumnsVisibilityTableWindowBase() {
        Loaded += TableWindowBase_Loaded;
    }
    
    protected void TableWindowBase_Loaded(object sender, RoutedEventArgs e) {
        // Отписываемся, чтобы событие не срабатывало повторно
        Loaded -= TableWindowBase_Loaded;

        // Автоматически находим оба компонента на форме дочернего окна
        var comboBox = FindName("ColumnsVisibilityComboBox") as ComboBox;

        // Если компоненты найдены, восстанавливаем состояние колонок
        var dg = VisualHelper.FindVisualChild<DataGrid>(this);
        if (dg == null) {
            Console.Error.WriteLine("No DataGrid found");
            throw new Exception("No DataGrid found");
        }
        ColumnVisibilitySettings.LoadColumnsVisibility(dg, GetType().ToString(), ColumnsVisibilityComboBox);
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
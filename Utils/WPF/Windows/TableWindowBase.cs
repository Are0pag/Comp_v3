using System.Windows;
using System.Windows.Controls;
using WPF.Extensions.View.Elements;

namespace Utils.WPF.Windows;

public class TableWindowBase : Window
{
    public TableWindowBase() {
        Closed += (sender, args) => {
            try {
                // Находим DataGrid на форме через логическое дерево
                var dataGrid = FindVisualChild<DataGrid>(this);
                if (dataGrid != null && dataGrid.IsEditing()) {
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        };
    }

    private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject {
        for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++) {
            var child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
            if (child is T t)
                return t;
            var childOfChild = FindVisualChild<T>(child);
            if (childOfChild != null)
                return childOfChild;
        }

        return null;
    }
}
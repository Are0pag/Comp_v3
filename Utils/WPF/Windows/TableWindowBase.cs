using System.Windows;
using System.Windows.Controls;
using WPF.Extensions.View.Elements;

namespace Utils.WPF.Windows;

public class TableWindowBase : PosWindowBase
{
    public TableWindowBase() {
        Closed += (sender, args) => {
            try {
                // Находим DataGrid на форме через логическое дерево
                var dataGrid = VisualHelper.FindVisualChild<DataGrid>(this);
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


}

public class WindowBase : Window
{

}
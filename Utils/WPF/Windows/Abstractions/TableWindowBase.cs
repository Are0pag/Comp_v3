using System.Windows;
using System.Windows.Controls;
using WPF.Extensions.View.Elements;

namespace Utils.WPF.Windows;

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
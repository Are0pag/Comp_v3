using System.Windows.Input;
using System.Windows.Threading;

namespace Comp_v3.Front.DataGrid.CondDesign.RoutedCommands;

public class GridLastCellEditEndingRoutedCommand : RoutedCommandBase 
{
    protected override KeyGesture SetKeyGesture() {
        return new KeyGesture(Key.Enter, ModifierKeys.None);
    }

    protected override void Execute(object sender, ExecutedRoutedEventArgs e) {
        if (sender is not System.Windows.Controls.DataGrid grid)
            return;

        if (!IsLastCell(grid)) return;
        
        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => 
        {
            grid.Focus();
            grid.CancelEdit();
        }), DispatcherPriority.Input);
    }

    protected override void CanExecute(object sender, CanExecuteRoutedEventArgs e) {
        e.CanExecute = true;
    }

    protected bool IsLastCell(System.Windows.Controls.DataGrid grid) {
        return grid.CurrentCell.Column == grid.Columns.LastOrDefault() 
               && 
               grid.Items.IndexOf(grid.CurrentItem) == grid.Items.Count - 1;
    }
}
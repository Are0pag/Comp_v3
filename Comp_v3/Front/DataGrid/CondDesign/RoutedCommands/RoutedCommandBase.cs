using System.Windows.Input;

namespace Comp_v3.Front.DataGrid.CondDesign.RoutedCommands;

public abstract class RoutedCommandBase
{
    public required CommandBinding CommandBinding { get; init; }
    public required InputBinding InputBinding { get; init; }

    public RoutedCommandBase() {
        RoutedCommand cmd = new RoutedCommand();
        CommandBinding = new CommandBinding(cmd, Execute, CanExecute);
        InputBinding = new InputBinding(cmd, SetKeyGesture());
    }
    protected abstract KeyGesture SetKeyGesture();
    protected abstract void Execute(object sender, ExecutedRoutedEventArgs e);
    protected abstract void CanExecute(object sender, CanExecuteRoutedEventArgs e);
}

public class CancelCellEditingRoutedCommand : RoutedCommandBase
{
    protected override KeyGesture SetKeyGesture() {
        return new KeyGesture(Key.Escape, ModifierKeys.None);
    }

    protected override void Execute(object sender, ExecutedRoutedEventArgs e) {
        throw new NotImplementedException();
    }

    protected override void CanExecute(object sender, CanExecuteRoutedEventArgs e) {
        throw new NotImplementedException();
    }
}
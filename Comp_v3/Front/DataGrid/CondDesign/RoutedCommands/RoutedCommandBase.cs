using System.Windows.Input;

namespace Comp_v3.Front.DataGrid.CondDesign.RoutedCommands;

public abstract class RoutedCommandBase
{
    protected RoutedCommand _command;
    public RoutedCommandBase() {
        CreateRoutedCommand();
    }
    
    protected abstract KeyGesture SetKeyGesture();

    public virtual List<InputBinding> GetInputBindings() {
        return new List<InputBinding>() {
            new InputBinding(_command, SetKeyGesture())
        };
    }
    public virtual List<CommandBinding> GetCommandBindings() {
        return new List<CommandBinding>() {
            new CommandBinding(_command, Execute, CanExecute)
        };
    }
    protected virtual void CreateRoutedCommand() {
        _command = new RoutedCommand();
    }

    protected abstract void Execute(object sender, ExecutedRoutedEventArgs e);
    protected abstract void CanExecute(object sender, CanExecuteRoutedEventArgs e);
}

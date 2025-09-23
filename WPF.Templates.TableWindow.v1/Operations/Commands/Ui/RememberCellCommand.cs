using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberCellCommand<TWindow, T> : BaseCommand<RememberCellCommand<TWindow, T>.Args>
    where TWindow : Window
    where T : class
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;
    
    protected DataGridCell? _cell;
    
    public RememberCellCommand(Args parameter, ModuleContext<TWindow, T> moduleContext) : base(parameter) {
        _moduleContext = moduleContext;
    }

    public override Task ExecuteAsync() {
        _cell = _moduleContext.DataGrid.GetCell(_parameter.EventArgs.Row, _parameter.EventArgs.Column);
        return Task.CompletedTask;
    }

    public override async Task UndoAsync() {
        await Task.Delay(100);
        await _parameter.Dispatcher.InvokeAsync(() => {
            try {
                _cell!.Focus();
                _moduleContext.DataGrid.SelectedItem = _cell;
                _moduleContext.DataGrid.BeginEdit();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }

    public class Args
    {
        public Args(DataGridBeginningEditEventArgs dataGridBeginningEditEventArgs, Dispatcher dispatcher) {
            EventArgs = dataGridBeginningEditEventArgs;
            Dispatcher = dispatcher;
        }

        public DataGridBeginningEditEventArgs EventArgs { get; }
        public System.Windows.Threading.Dispatcher Dispatcher { get; }
    }
}
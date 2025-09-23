using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberCellCommand<TWindow, T> : BaseCommand<DataGridBeginningEditEventArgs>
    where TWindow : Window
    where T : class
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;
    protected readonly System.Windows.Threading.Dispatcher _dispatcher;
    
    protected DataGridCell? _cell;
    
    public RememberCellCommand(DataGridBeginningEditEventArgs parameter, ModuleContext<TWindow, T> moduleContext, Dispatcher dispatcher) : base(parameter) {
        _moduleContext = moduleContext;
        _dispatcher = dispatcher;
    }

    public override Task ExecuteAsync() {
        _cell = _moduleContext.DataGrid.GetCell(_parameter.Row, _parameter.Column);
        return Task.CompletedTask;
    }

    public override async Task UndoAsync() {
        await Task.Delay(100);
        await _dispatcher.InvokeAsync(() => {
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
            DataGridBeginningEditEventArgs = dataGridBeginningEditEventArgs;
            Dispatcher = dispatcher;
        }

        public DataGridBeginningEditEventArgs DataGridBeginningEditEventArgs { get; }
        public System.Windows.Threading.Dispatcher Dispatcher { get; }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPF.Extensions.View.Elements;
using WPF.Templates.TableWindow.v1.Entities;

namespace WPF.Templates.TableWindow.v1.Operations.Commands.Ui;

public class RememberCellCommand<TWindow, T> : BaseCommand<RememberCellCommand<TWindow, T>.Args>
    where TWindow : Window
    where T : class
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;
    protected DataGridCell? _cell;
#if DEBUG
    protected int _rowIndex;
    protected string _columnHeader;
#endif

    
    public RememberCellCommand(Args parameter, ModuleContext<TWindow, T> moduleContext) : base(parameter) {
        _moduleContext = moduleContext;
    }

    public override Task ExecuteAsync() {
        _cell = _moduleContext.DataGrid.GetCell(_parameter.EventArgs.Row, _parameter.EventArgs.Column);
        
    #if DEBUG
        _columnHeader = _cell.Column.Header.ToString();
        _rowIndex = _parameter.EventArgs.Row.GetIndex();
        Console.WriteLine($"Cell Details:");
        Console.WriteLine($"  {"Row Index:",-20} {_parameter.EventArgs.Row.GetIndex()}");
        Console.WriteLine($"  {"Column Index:",-20} {_parameter.EventArgs.Column.DisplayIndex}");
        Console.WriteLine($"  {"Column Header:",-20} {_cell.Column.Header}");
        Console.WriteLine($"  {"Column Display Index:",-20} {_cell.Column.DisplayIndex}");
    #endif
        return Task.CompletedTask;
    }

    public override async Task UndoAsync() {
        await Task.Delay(100);
        Dispatcher.CurrentDispatcher.BeginInvoke(() => {
            _cell!.Focus();
            _moduleContext.DataGrid.SelectedItem = _cell;
            _moduleContext.DataGrid.BeginEdit();
        });
        /*await _parameter.Dispatcher.InvokeAsync(() => {
            try {
                _cell!.Focus();
                _moduleContext.DataGrid.SelectedItem = _cell;
                _moduleContext.DataGrid.BeginEdit();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });*/
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
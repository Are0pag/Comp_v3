using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberCellCommand : BaseCommand
{
    protected readonly Cell _cell;
    protected DataGridCellInfo _cellInfo;
    
    public RememberCellCommand(ModuleContext context) : base(context) {
        _cell = App.Host.Services.GetRequiredService<Cell>();
    }

    public override async Task ExecuteAsync() {
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                _cellInfo = _context.DataGrid.CurrentCell;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }

    public override async Task UndoAsync() {
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                _context.DataGrid.CurrentCell = _cellInfo;
                _context.DataGrid.Focus();
                _context.DataGrid.BeginEdit();
                
                var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) {
                    RoutedEvent = Mouse.MouseDownEvent
                };
        
                _context.DataGrid.RaiseEvent(args);
                args.RoutedEvent = Mouse.MouseUpEvent;
                _context.DataGrid.RaiseEvent(args);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }
}
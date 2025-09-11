using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberCellCommand : BaseCommand
{
    protected DataGridColumn _column;

    public RememberCellCommand(ModuleContext context, object? parameter) : base(context, parameter) { }

    public override async Task ExecuteAsync() {
        await Task.Delay(100);
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                _column = _context.DataGrid.CurrentCell.Column;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }

    public override async Task UndoAsync() {
        await Task.Delay(100);
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                var dg = _context.DataGrid;

                if (_parameter is not DataGridRow raw)
                    throw new NullReferenceException();
                
                var cell = dg.GetCell(raw, _column);
                if (cell == null)
                    throw new NullReferenceException();
                
                cell.Focus();
                dg.BeginEdit();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }
}
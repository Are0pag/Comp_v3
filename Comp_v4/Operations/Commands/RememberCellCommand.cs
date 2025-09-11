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
    protected readonly Cell _cell;
    protected DataGridCellInfo _cellInfo;
    protected DataGridColumn _column;
    protected ConditionalDesignation? _item;
    
    public RememberCellCommand(ModuleContext context) : base(context) {
        _cell = App.Host.Services.GetRequiredService<Cell>();
    }

    public override async Task ExecuteAsync() {
        await Task.Delay(100);
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                _item = Item;
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
                var rowFromItem = dg.GetRowFromItem(_item!);
                if (rowFromItem == null) 
                    throw new NullReferenceException();
                var cell = dg.GetCell(rowFromItem, _column);
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
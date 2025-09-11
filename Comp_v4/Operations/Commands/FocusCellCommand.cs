using System.Windows.Controls;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class FocusCellCommand : BaseCommand
{
    protected readonly Cell _cell;
    public FocusCellCommand(ModuleContext context) : base(context) {
        _cell = App.Host.Services.GetRequiredService<Cell>();
    }

    public override Task ExecuteAsync() {
        //_cell.IsEnabled = true;
        var dg = _context.DataGrid;
        var column = dg.GetFirstEditableColumn();
        
        Dispatcher.CurrentDispatcher.BeginInvoke(() => {
            var raw = dg.GetRowFromItem(_item!);
            var cell = dg.GetCell(raw, column);

            dg.CurrentCell = new DataGridCellInfo(_item!, column);
            cell.Focus();
            dg.BeginEdit();
            //_cell.IsEnabled = false;
        }, DispatcherPriority.ContextIdle);
        return Task.CompletedTask;
    }
}
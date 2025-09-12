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

    public FocusCellCommand(ModuleContext context, object? parameter = null) : base(context, parameter) {
        _cell = App.Host.Services.GetRequiredService<Cell>();
    }

    public override Task ExecuteAsync() {
        var dg = _context.DataGrid;
        
        Dispatcher.CurrentDispatcher.BeginInvoke(() => {
            var column = dg.GetFirstEditableColumn();
            var raw = dg.GetRowFromItem(Item!);
            var cell = dg.GetCell(raw, column);

            dg.CurrentCell = new DataGridCellInfo(Item!, column);
            cell.Focus();
            dg.BeginEdit();
        }, DispatcherPriority.ContextIdle);
        return Task.CompletedTask;
    }
}
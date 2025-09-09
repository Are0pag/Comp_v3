using System.Windows.Controls;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class FocusCellCommand : BaseCommand
{
    protected readonly DataGridCellEditEventHandler _cellEditEventHandler;
    public FocusCellCommand(ModuleContext context, ConditionalDesignation item) : base(context) {
        _item = item;
        _cellEditEventHandler = App.Host.Services.GetRequiredService<DataGridCellEditEventHandler>();
    }

    public override async Task ExecuteAsync() {
        _cellEditEventHandler.IsEnabled = true;
        var dg = _context.DataGrid;
        var column = dg.GetFirstEditableColumn();
        
        Dispatcher.CurrentDispatcher.BeginInvoke(() => {
            var raw = dg.GetRowFromItem(_item!);
            var cell = dg.GetCell(raw, column);

            dg.CurrentCell = new DataGridCellInfo(_item!, column);
            cell.Focus();
            dg.BeginEdit();
            _cellEditEventHandler.IsEnabled = false;
        }, DispatcherPriority.ContextIdle);
    }

    public override async Task UndoAsync() {
        
    }

    public override async Task ExecuteDeferredAsync() {
       
    }
}
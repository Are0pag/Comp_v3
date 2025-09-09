using System.Windows.Controls;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class FocusCellCommand : BaseCommand
{
    public FocusCellCommand(ModuleContext context, ConditionalDesignation item) : base(context) {
        _item = item;
    }

    public override async Task ExecuteAsync() {
        var dg = _context.DataGrid;
        var column = dg.GetFirstEditableColumn();
        
        Dispatcher.CurrentDispatcher.BeginInvoke(() => {
            var raw = dg.GetRowFromItem(_item!);
            var cell = dg.GetCell(raw, column);

            dg.CurrentCell = new DataGridCellInfo(_item!, column);
            cell.Focus();
            dg.BeginEdit();
        }, DispatcherPriority.ContextIdle);
    }

    public override async Task UndoAsync() {
        
    }

    public override async Task ExecuteDeferredAsync() {
       
    }
}
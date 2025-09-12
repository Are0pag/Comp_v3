using System.Windows.Controls;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class FocusCellCommand : BaseCommand<ConditionalDesignation>
{
    public FocusCellCommand(ConditionalDesignation parameter) : base(parameter) {
    }

    public override async Task ExecuteAsync() {
        var dg = _moduleContext.DataGrid;
        Dispatcher.CurrentDispatcher.BeginInvoke(async () => {
            try {
                var column = dg.GetFirstEditableColumn();
                await Task.Delay(200);
                var raw = await dg.GetRowFromItemAsync(_parameter!);
                var cell = dg.GetCell(raw, column);

                dg.CurrentCell = new DataGridCellInfo(_parameter!, column);
                cell.Focus();
                dg.BeginEdit();
            }
            catch (Exception e) {
                e.Log(this);
                throw;
            }
        }, DispatcherPriority.ContextIdle);
        return;
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class FocusCellCommand<TWindow, T> : BaseCommand<T>
    where TWindow : Window
    where T : class, new()
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;
    public FocusCellCommand(T parameter, ModuleContext<TWindow, T> moduleContext) : base(parameter) {
        _moduleContext = moduleContext;
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
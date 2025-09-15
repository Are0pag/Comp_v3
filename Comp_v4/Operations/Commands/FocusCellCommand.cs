using System.Windows.Controls;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm;

namespace Comp_v4.Operations.Commands;

public class FocusCellCommand : BaseCommand<ConditionalDesignation>
{
    public FocusCellCommand(ConditionalDesignation parameter) : base(parameter) {
    }

    public override async Task ExecuteAsync() {
        var dg = _moduleContext.DataGrid;

        if (_moduleContext.DataGridViewModel is DataGridViewModelFiltered filteredViewModel && !filteredViewModel.FilteredItems.Contains(_parameter)) {
            filteredViewModel.ClearFiltersCommand.Execute(null);
            await Task.Delay(100);
        }

        dg.ScrollIntoView(_parameter);

        Dispatcher.CurrentDispatcher.BeginInvoke(() => {
            try {
                var column = dg.GetFirstEditableColumn();
                dg.CurrentCell = new DataGridCellInfo(_parameter!, column);
                dg.BeginEdit();
            }
            catch (Exception e) {
                e.Log(this);
                throw;
            }
        }, DispatcherPriority.ContextIdle);
    }
}
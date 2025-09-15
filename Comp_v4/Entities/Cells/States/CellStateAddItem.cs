using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.TableWindow.States;

public class CellStateAddItem : BaseCellStateInput
{
    public CellStateAddItem(IModuleCommandScheduler scheduler, ModuleContext context, Validator validator, ActionAddItem actionAddItem) : base(scheduler, context, validator) {
        _action = actionAddItem;
    }

    public override Task Enter(Cell context) {
        ((DataGridViewModelFiltered)_context.DataGridViewModel).ClearFilters();
        return base.Enter(context);
    }

    public override Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        _rememberCellCommand = new RememberCellCommand(e);
        _lastCellEditBeginningEditEventArgs = e;
        return Task.CompletedTask;
    }
}
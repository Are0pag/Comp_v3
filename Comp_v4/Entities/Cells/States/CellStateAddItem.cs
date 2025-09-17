using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;

namespace WPF.Templates.TableWindow.States;

public class CellStateAddItem : BaseCellStateInput
{
    public CellStateAddItem(IModuleCommandScheduler scheduler, ModuleContext context, CommandFactory factory, Validator validator, ActionAddItem actionAddItem) 
        : base(scheduler, context, factory, validator) {
        _action = actionAddItem;
    }

    public override Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        _rememberCellCommand = _commandFactory.CreateCommand<RememberCellCommand, DataGridBeginningEditEventArgs>(e);
        _lastCellEditBeginningEditEventArgs = e;
        return Task.CompletedTask;
    }
}
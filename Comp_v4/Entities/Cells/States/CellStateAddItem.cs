using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.Entities;
using WPF.Extensions.View.Elements;

namespace WPF.Templates.TableWindow.States;

public class CellStateAddItem : BaseCellState
{
    protected readonly ActionAddItem _actionAddItem;
    protected DataGridBeginningEditEventArgs? _lastCellEditBeginningEditEventArgs;

    public CellStateAddItem(IModuleCommandScheduler scheduler, ModuleContext context, ActionAddItem actionAddItem) : base(scheduler, context) {
        _actionAddItem = actionAddItem;
    }

    public override Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        _lastCellEditBeginningEditEventArgs = e;
        return Task.CompletedTask;
    }

}
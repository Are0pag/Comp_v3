using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;

namespace WPF.Templates.TableWindow.States;

public class CellStateIdle : BaseCellState
{
    public CellStateIdle(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override async Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        var targetState = owner.GetState<CellStateInput>();

        _scheduler.BeginTransaction<TrSelectingCell>();

        await _scheduler.RegisterCommandInto<TrSelectingCell>(new CellChangeStateCommand(_context, owner, targetState) {
            Description = "Id = 1. Maybe will have logic to remove any focus and selection on Undo"
        }).ExecuteLastRegisteredAsync();
        
        await targetState.OnBeginning(owner, sender, e);
    }
}
using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.States;

public class CellStateIdle : BaseCellState
{
    public CellStateIdle(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override async Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        var targetState = owner.GetState<CellStateInput>();
        
        await new CellChangeStateCommand(_context, owner, targetState).ExecuteAsync();
        /*await _scheduler.BeginTransaction<TransactionUpdateItem>()
                        .RegisterCommand(new CellChangeStateCommand(_context, owner, targetState))
                        .ExecuteLastRegisteredAsync();*/

        await targetState.OnBeginning(owner, sender, e);
    }
}
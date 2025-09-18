using System.Windows;
using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Infrastructure.Command;

namespace WPF.Templates.TableWindow.States;

public class CellStateIdle<TWindow, T> : BaseCellState<TWindow, T>
    where TWindow : Window
    where T : class
{
    public CellStateIdle(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory factory) : base(scheduler, context, factory) {
    }

    public override async Task OnBeginning(Cell<TWindow, T> owner, object? sender, DataGridBeginningEditEventArgs e) {
        _scheduler.BeginTransaction<TrSelectingCell>();
        
        var targetState = owner.GetState<CellStateUpdate<TWindow, T>>();
        await _scheduler.RegisterCommandInto<TrSelectingCell>(new CellChangeStateCommand(_context, owner, targetState) {
            Description = "Id = 1. Maybe will have logic to remove any focus and selection on Undo"
        }).ExecuteLastRegisteredAsync();
        
        await targetState.OnBeginning(owner, sender, e);
    }
}
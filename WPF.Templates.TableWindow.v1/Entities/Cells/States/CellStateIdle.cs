using System.Windows;
using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Templates.TableWindow.v1.Operations.Commands;
using WPF.Templates.TableWindow.v1.Operations.Transactions;

namespace WPF.Templates.TableWindow.v1.Entities.Cells.States;

public class CellStateIdle<TWindow, T> : BaseCellState<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity
{
    public CellStateIdle(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory factory) : base(scheduler, context, factory) {
    }

    public override async Task OnBeginning(Cell<TWindow, T> owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (_scheduler.IsInTransaction<TrSelectingCell>())
            return;
        
        _scheduler.BeginTransaction<TrSelectingCell>();
        
        var targetState = owner.GetState<CellStateUpdate<TWindow, T>>();
        await _scheduler.RegisterCommandInto<TrSelectingCell>(new CellChangeStateCommand<TWindow, T>(_context, owner, targetState) {
            Description = "Id = 1. Maybe will have logic to remove any focus and selection on Undo"
        }).ExecuteLastRegisteredAsync();
        
        await targetState.OnBeginning(owner, sender, e);
    }
}
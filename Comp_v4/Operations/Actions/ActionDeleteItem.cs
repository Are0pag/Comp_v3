using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class ActionDeleteItem : BaseAction
{
    protected readonly Cell _cell;
    protected new readonly ModuleContext _context;
    
    public ActionDeleteItem(IModuleCommandScheduler scheduler, ModuleContext context, Cell cell) : base(scheduler, context) {
        _context = context;
        _cell = cell;
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        if (_context.DataGrid.SelectedItem is not ConditionalDesignation item) {
            new ArgumentException().Log(this);
            return this;
        }
        
        _scheduler.BeginTransaction<TrDeleteCell>();
        _scheduler.RegisterCommandInto<TrDeleteCell>(new DeleteItemCommand(item));
        await _scheduler.RegisterCommandInto<TrDeleteCell>(new RemoveItemCommand(item))
                        .ExecuteLastRegisteredAsync();
        _scheduler.CommitTransaction<TrDeleteCell>();
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is CellStateIdle && _context.DataGrid.SelectedItem != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
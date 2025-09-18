using System.Windows;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using Infrastructure.Command;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class ActionDeleteItem<TWindow, T> : BaseAction<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly Cell<TWindow, T> _cell;
    protected new readonly ModuleContext<TWindow, T> _context;
    
    public ActionDeleteItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory, Cell<TWindow, T> cell) : base(scheduler, context, commandFactory) {
        _context = context;
        _cell = cell;
    }

    public override async Task<BaseAction<TWindow, T> > PerformAsync(object? parameter = null) {
        if (_context.DataGrid.SelectedItem is not T item) {
            new ArgumentException().Log(this);
            return this;
        }
        
        _scheduler.BeginTransaction<TrDeleteCell>();
        _scheduler.RegisterCommandInto<TrDeleteCell>(_commandFactory.CreateCommand<DeleteItemCommand<T>, T>(item));
        await _scheduler.RegisterCommandInto<TrDeleteCell>(_commandFactory.CreateCommand<RemoveItemCommand<TWindow, T>, T>(item))
                        .ExecuteLastRegisteredAsync();
        _scheduler.CommitTransaction<TrDeleteCell>();
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is CellStateIdle<TWindow, T>  && _context.DataGrid.SelectedItem != null;
    }

    public override Task CancelAsync(object? parameter = null) {
        return Task.CompletedTask;
    }
}
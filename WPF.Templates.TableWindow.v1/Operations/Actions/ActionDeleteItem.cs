using System.Windows;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using Infrastructure.Command;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Entities.Cells.States;
using WPF.Templates.TableWindow.v1.Events.Update;
using WPF.Templates.TableWindow.v1.Operations.Commands.Db;
using WPF.Templates.TableWindow.v1.Operations.Commands.Ui;
using WPF.Templates.TableWindow.v1.Operations.Transactions;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

public class ActionDeleteItem<TWindow, T> : BaseAction<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly Cell<TWindow, T> _cell;
    protected readonly IRepository<T> _repository;
    public ActionDeleteItem(IDataGridCommandScheduler scheduler, 
                            ModuleContext<TWindow, T> context, 
                            ICommandFactory commandFactory, 
                            Cell<TWindow, T> cell, 
                            IRepository<T> repository) 
        : base(scheduler, context, commandFactory) {
        _cell = cell;
        _repository = repository;
    }

    public override async Task<BaseAction<TWindow, T> > PerformAsync(object? parameter = null) {
        if (_context.DataGrid.SelectedItem is not T item) {
            new ArgumentException().Log(this);
            return this;
        }
        
        _scheduler.BeginTransaction<TrDeleteCell>();
        _scheduler.RegisterCommandInto<TrDeleteCell>(new DeleteItemCommand<T>(item, _repository));
        await _scheduler.RegisterCommandInto<TrDeleteCell>(new RemoveItemCommand<TWindow, T>(item, _context))
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
using System.Windows;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using Infrastructure.Command;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Entities.Cells.States;
using WPF.Templates.TableWindow.v1.Operations.Commands;
using WPF.Templates.TableWindow.v1.Operations.Commands.Db;
using WPF.Templates.TableWindow.v1.Operations.Commands.Ui;
using WPF.Templates.TableWindow.v1.Operations.Transactions;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

public class ActionUpdateItem<TWindow, T> : BaseAction<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly IRepository<T> _repository;
    public ActionUpdateItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory, IRepository<T> repository) : base(scheduler, context, commandFactory) {
        _repository = repository;
    }

    public override async Task<BaseAction<TWindow, T>> PerformAsync(object? parameter = null) {
        if (parameter is not Args args) {
            new InvalidOperationException().Log(this);
            return this;
        }
        
        _scheduler.BeginTransaction<TrEditCell>();

        await InitTransaction(args);
        await _scheduler.RegisterCommandInto<TrEditCell>(new CellChangeStateCommand<TWindow, T>(_context, args.Cell, args.Cell.GetState<CellStateIdle<TWindow, T>>()))
                        .ExecuteLastRegisteredAsync();
        _scheduler.CommitTransaction<TrEditCell>();
        return this;
    }

    protected virtual Task InitTransaction(Args args) {
        _scheduler.RegisterCommandInto<TrEditCell>(args.RememberCellCommand);
        _scheduler.RegisterCommandInto<TrEditCell>(new UpdateItemCommand<T>(args.Item, _repository));
        return Task.CompletedTask;
    }

    public override bool CanPerform() {
        return true;
    }

    public override async Task CancelAsync(object? parameter = null) {
        await _scheduler.UndoAsync();
        _context.DataGrid.CancelEdit();
    }

    public class Args(RememberCellCommand<TWindow, T> rememberCellCommand, Cell<TWindow, T> cell, T item) {
        public RememberCellCommand<TWindow, T> RememberCellCommand { get; set; } = rememberCellCommand;
        public Cell<TWindow, T> Cell { get; set; } = cell;
        public T Item {get; set;} = item;
    }
}
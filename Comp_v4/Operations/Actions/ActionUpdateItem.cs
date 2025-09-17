using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class ActionUpdateItem : BaseAction
{
    public ActionUpdateItem(IModuleCommandScheduler scheduler, ModuleContext context, CommandFactory commandFactory) : base(scheduler, context, commandFactory) {
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        if (parameter is not Args args) {
            new InvalidOperationException().Log(this);
            return this;
        }
        
        _scheduler.BeginTransaction<TrEditCell>();

        await InitTransaction(args);
        await _scheduler.RegisterCommandInto<TrEditCell>(new CellChangeStateCommand(_context, args.Cell, args.Cell.GetState<CellStateIdle>()))
                        .ExecuteLastRegisteredAsync();
        _scheduler.CommitTransaction<TrEditCell>();
        return this;
    }

    protected virtual Task InitTransaction(Args args) {
        _scheduler.RegisterCommandInto<TrEditCell>(args.RememberCellCommand);
        _scheduler.RegisterCommandInto<TrEditCell>(_commandFactory.CreateCommand<UpdateItemCommand, ConditionalDesignation>(args.Item));
        return Task.CompletedTask;
    }

    public override bool CanPerform() {
        return true;
    }

    public override async Task CancelAsync(object? parameter = null) {
        //_context.DataGrid.CurrentCell.
        await _scheduler.UndoAsync();
        _context.DataGrid.CancelEdit();
    }

    public class Args(RememberCellCommand rememberCellCommand, Cell cell, ConditionalDesignation item) {
        public RememberCellCommand RememberCellCommand { get; set; } = rememberCellCommand;
        public Cell Cell { get; set; } = cell;
        public ConditionalDesignation Item {get; set;} = item;
    }
}
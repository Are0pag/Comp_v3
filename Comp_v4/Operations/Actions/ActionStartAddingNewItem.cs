using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class ActionStartAddingNewItem : BaseAction
{
    protected readonly Cell _cell;
    
    public ActionStartAddingNewItem(IModuleCommandScheduler scheduler, ModuleContext context, Cell cell) : base(scheduler, context) {
        _cell = cell;
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        _scheduler.BeginTransaction<TransactionAddItem>();
        
        var createRawCommand = new CreateRawCommand(_context);

        await _scheduler.RegisterCommandInto<TransactionAddItem>(new CellChangeStateCommand(_context, _cell, _cell.GetState<CellStateAddItem>()))
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TransactionAddItem>(new RememberSelectionCommand(_context))
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<TransactionAddItem>(createRawCommand)
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<TransactionAddItem>(new FocusCellCommand(createRawCommand.Item))
                        .ExecuteLastRegisteredAsync();

        _scheduler.CommitTransaction<TransactionAddItem>();
        
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is CellStateIdle;
    }

    public override async Task CancelAsync(object? parameter = null) {
        await _scheduler.CommitTransaction<TransactionAddItem>().UndoAsync();
    }
}


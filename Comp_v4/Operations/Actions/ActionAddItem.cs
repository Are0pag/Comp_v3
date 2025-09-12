using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class ActionAddItem : BaseAction
{
    protected readonly Cell _cell;
    
    public ActionAddItem(IModuleCommandScheduler scheduler, ModuleContext context, Cell cell) : base(scheduler, context) {
        _cell = cell;
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        await _scheduler.BeginTransaction<TransactionAddItem>()
                        .RegisterCommand(new RememberSelectionCommand(_context))
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TransactionAddItem>(new CreateRawCommand(_context))
                        .ExecuteLastRegisteredAsync();

        _scheduler.RegisterCommandInto<TransactionAddItem>(new AddItemCommand(null));

        await _scheduler.RegisterCommandInto<TransactionAddItem>(new FocusCellCommand(_context))
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<TransactionAddItem>(new CellChangeStateCommand(_context, _cell, _cell.GetState<CellStateInput>()))
                        .ExecuteLastRegisteredAsync();

        _scheduler.CommitTransaction<TransactionAddItem>();
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is not CellStateInput;
    }

    public override async Task CancelAsync(object? parameter = null) {
        await _scheduler.CommitTransaction<TransactionAddItem>().UndoAsync();
    }
}
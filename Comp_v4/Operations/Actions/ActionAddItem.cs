using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates;

public class ActionAddItem : BaseAction
{
    protected readonly Cell _cell;
    
    public ActionAddItem(HeterochromicCommandScheduler scheduler, ModuleContext context, Cell cell) : base(scheduler, context) {
        _cell = cell;
    }

    public override async Task<BaseAction> PerformAsync() {
        await _scheduler.BeginTransaction<TransactionAddItem>()
                        .RegisterCommand(new RememberSelectionCommand(_context))
                        .ExecuteLastRegisteredAsync();

        var createRawCommand = new CreateRawCommand(_context);
        
        await _scheduler.RegisterCommandInto<TransactionAddItem>(createRawCommand)
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<TransactionAddItem>(new FocusCellCommand(_context, createRawCommand.Item))
                        .ExecuteLastRegisteredAsync();

        _scheduler.CommitTransaction<TransactionAddItem>();
        return this;
    }

    public override bool CanPerform() {
        return false;
    }

    public override async Task CancelAsync() {
        await _scheduler.UndoAsync();
    }
}
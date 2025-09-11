using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;

namespace WPF.Templates;

public class ActionUpdateItem : BaseAction
{
    public ActionUpdateItem(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override async Task<BaseAction> PerformAsync() {
        await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new UpdateItemCommand(_context))
                        .ExecuteLastRegisteredAsync();
        return this;
    }

    public override bool CanPerform() {
        return true;
    }

    public override async Task CancelAsync() {
        await _scheduler.CommitTransaction<TransactionUpdateItem>().UndoAsync();
    }
}
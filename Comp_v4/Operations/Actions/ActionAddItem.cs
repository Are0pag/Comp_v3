using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates;

public class ActionAddItem : BaseAction
{
    public ActionAddItem(HeterochromicCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override async Task<BaseAction> PerformAsync() {
        await _scheduler.BeginTransaction<TransactionAddItem>()
                        .RegisterCommand(new RememberSelection(_context))
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TransactionAddItem>(new CreateRawCommand(_context))
                        .ExecuteLastRegisteredAsync();
        
        return this;
    }

    public override bool CanPerform() {
        throw new NotImplementedException();
    }

    public override async Task CancelAsync() {
        throw new NotImplementedException();
    }
}
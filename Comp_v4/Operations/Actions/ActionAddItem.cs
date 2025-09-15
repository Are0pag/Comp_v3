using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;

namespace WPF.Templates;

public class ActionAddItem : ActionUpdateItem
{
    public ActionAddItem(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    protected override async Task InitTransaction(Args args) {
        await _scheduler.RegisterCommandInto<TrEditCell>(args.RememberCellCommand)
                        .ExecuteLastRegisteredAsync();
        _scheduler.RegisterCommandInto<TrEditCell>(new UpdateItemCommand(args.Item));
    }
}
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;

namespace WPF.Templates;

public class ActionAddItem : ActionUpdateItem
{
    public ActionAddItem(IModuleCommandScheduler scheduler, ModuleContext context, CommandFactory commandFactory) : base(scheduler, context, commandFactory) {
    }

    protected override async Task InitTransaction(Args args) {
        await _scheduler.RegisterCommandInto<TrEditCell>(args.RememberCellCommand)
                        .ExecuteLastRegisteredAsync();
        
        _scheduler.RegisterCommandInto<TrEditCell>(_commandFactory.CreateCommand<UpdateItemCommand, ConditionalDesignation>(args.Item));
    }
}
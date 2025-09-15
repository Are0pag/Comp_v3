using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;

namespace WPF.Templates;

public class ActionAddItem : ActionUpdateItem
{
    public ActionAddItem(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    protected override void SaveToDb(ConditionalDesignation cd) {
        _scheduler.RegisterCommandInto<TrEditCell>(new AddItemCommand(cd));
    }
}
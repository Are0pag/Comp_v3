using Comp_v4.Entities;

namespace WPF.Templates.TableWindow.States;

public class CellStateUpdate : BaseCellStateInput
{
    public CellStateUpdate(IModuleCommandScheduler scheduler, ModuleContext context, ActionUpdateItem actionUpdateItem, Validator validator) : base(scheduler, context, validator) {
        _action = actionUpdateItem;
    }
}
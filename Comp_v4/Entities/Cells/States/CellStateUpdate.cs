using Comp_v4.Entities;
using Comp_v4.Operations.Commands;

namespace WPF.Templates.TableWindow.States;

public class CellStateUpdate : BaseCellStateInput
{
    public CellStateUpdate(IModuleCommandScheduler scheduler, ModuleContext context,  CommandFactory factory, ActionUpdateItem actionUpdateItem, Validator validator) 
        : base(scheduler, context, factory, validator) {
        _action = actionUpdateItem;
    }
}
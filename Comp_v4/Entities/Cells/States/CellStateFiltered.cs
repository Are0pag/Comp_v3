using Comp_v4.Entities;

namespace WPF.Templates.TableWindow.States;

public class CellStateFiltered : BaseCellStateInput
{
    public CellStateFiltered(IModuleCommandScheduler scheduler, ModuleContext context, Validator validator) : base(scheduler, context, validator) {
    }
    
}
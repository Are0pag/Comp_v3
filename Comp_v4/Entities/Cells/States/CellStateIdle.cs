using Comp_v4.Entities;
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.States;

public class CellStateIdle : BaseCellState
{
    public CellStateIdle(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }
}
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.States;

public class CellStateIdle : BaseCellState
{
    public CellStateIdle(HeterochromicCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }
}
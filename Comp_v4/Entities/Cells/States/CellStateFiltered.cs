using Comp_v4.Entities;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.TableWindow.States;

public class CellStateFiltered : BaseCellState
{
    public CellStateFiltered(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override Task Exit(Cell context) {
        ((DataGridViewModelFiltered)_context.DataGridViewModel).ClearFilters();
        return Task.CompletedTask;
    }
}
using System.ComponentModel;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands.Filtering;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm.Components;

namespace WPF.Templates;

public class ActionFilter : BaseAction
{
    protected readonly FiltersVm _filtersVm;
    protected readonly Cell _cell;
    
    public ActionFilter(IModuleCommandScheduler scheduler, ModuleContext context, FiltersVm filtersVm, Cell cell) : base(scheduler, context) {
        _filtersVm = filtersVm;
        _cell = cell;
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        var arg = new ApplyFilterCommand.Args(_context.DataGridViewModel.Items!, _filtersVm);
        await new ApplyFilterCommand(arg).ExecuteAsync();
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is CellStateIdle;
    }

    public override async Task CancelAsync(object? parameter = null) {
        await _scheduler.UndoAsync();
    }
}
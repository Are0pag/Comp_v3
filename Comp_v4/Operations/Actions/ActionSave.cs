using Comp_v4.Entities;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class ActionSave : BaseAction
{
    protected readonly Cell _cell;
    public ActionSave(IModuleCommandScheduler scheduler, ModuleContext context, Cell cell) : base(scheduler, context) {
        _cell = cell;
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        try {
            await _scheduler.CommitDeferredChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
        return this;
    }

    public override bool CanPerform() {
        return _scheduler.CanUndo() && _cell.CurrentState is CellStateIdle;
    }

    public override Task CancelAsync(object? parameter = null) {
        return Task.CompletedTask;
    }
}
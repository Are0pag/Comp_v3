using Comp_v4.Entities;

namespace WPF.Templates;

public class ActionSave : BaseAction
{
    public ActionSave(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
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
        return _scheduler.CanUndo();
    }

    public override Task CancelAsync(object? parameter = null) {
        return Task.CompletedTask;
    }
}
using Comp_v4.Entities;
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates;

public abstract class BaseAction
{
    protected readonly ModuleContext _context;
    protected readonly IModuleCommandScheduler _scheduler;

    protected BaseAction(IModuleCommandScheduler scheduler, ModuleContext context) {
        _scheduler = scheduler;
        _context = context;
    }

    public abstract Task<BaseAction> PerformAsync();

    public abstract bool CanPerform();
    
    public abstract Task CancelAsync();
}

public class ActionEditItem : BaseAction
{
    public ActionEditItem(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override async Task<BaseAction> PerformAsync() {
        throw new NotImplementedException();
    }

    public override bool CanPerform() {
        throw new NotImplementedException();
    }

    public override async Task CancelAsync() {
        throw new NotImplementedException();
    }
}

public class ActionDeleteItem : BaseAction
{
    public ActionDeleteItem(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override async Task<BaseAction> PerformAsync() {
        throw new NotImplementedException();
    }

    public override bool CanPerform() {
        throw new NotImplementedException();
    }

    public override async Task CancelAsync() {
        throw new NotImplementedException();
    }
}
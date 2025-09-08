using Infrastructure.Command.Heterochromic;

namespace WPF.Templates;

public abstract class BaseAction
{
    protected readonly HeterochromicCommandScheduler _scheduler;

    protected BaseAction(HeterochromicCommandScheduler scheduler) {
        _scheduler = scheduler;
    }

    public abstract Task<BaseAction> PerformAsync();

    public abstract bool CanPerform();
    
    public abstract Task CancelAsync();
}

public class ActionAddItem : BaseAction
{
    public ActionAddItem(HeterochromicCommandScheduler scheduler) : base(scheduler) {
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

public class ActionEditItem : BaseAction
{
    public ActionEditItem(HeterochromicCommandScheduler scheduler) : base(scheduler) {
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
    public ActionDeleteItem(HeterochromicCommandScheduler scheduler) : base(scheduler) {
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
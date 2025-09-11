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

    public abstract Task<BaseAction> PerformAsync(object? parameter = null);

    public abstract bool CanPerform();
    
    public abstract Task CancelAsync(object? parameter = null);
}

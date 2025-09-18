using Comp_v4.Entities;
using Comp_v4.Operations.Commands;

namespace WPF.Templates;

public abstract class BaseAction
{
    protected readonly ModuleContext _context;
    protected readonly IDataGridCommandScheduler _scheduler;
    protected readonly CommandFactory _commandFactory;

    protected BaseAction(IDataGridCommandScheduler scheduler, ModuleContext context, CommandFactory commandFactory) {
        _scheduler = scheduler;
        _context = context;
        _commandFactory = commandFactory;
    }

    public abstract Task<BaseAction> PerformAsync(object? parameter = null);

    public abstract bool CanPerform();
    
    public abstract Task CancelAsync(object? parameter = null);
}
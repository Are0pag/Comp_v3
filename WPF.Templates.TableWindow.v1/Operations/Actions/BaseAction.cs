using System.Windows;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Infrastructure.Command;

namespace WPF.Templates;

public abstract class BaseAction<TWindow, T>
    where TWindow : Window
    where T : class
{
    protected readonly ModuleContext<TWindow, T> _context;
    protected readonly IDataGridCommandScheduler _scheduler;
    protected readonly ICommandFactory _commandFactory;

    protected BaseAction(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory) {
        _scheduler = scheduler;
        _context = context;
        _commandFactory = commandFactory;
    }

    public abstract Task<BaseAction<TWindow, T>> PerformAsync(object? parameter = null);

    public abstract bool CanPerform();
    
    public abstract Task CancelAsync(object? parameter = null);
}
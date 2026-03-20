using System.Windows;
using Infrastructure.Command;
using WPF.Templates.TableWindow.v1.Entities;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

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
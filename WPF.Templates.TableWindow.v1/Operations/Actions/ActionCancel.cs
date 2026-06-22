using System.Windows;
using Comp.ModelData.Contracts;
using Infrastructure.Command;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Vm;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

public class ActionCancel<TWindow, T> : BaseAction<TWindow, T> 
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly DataGridViewModel<T> _dgVm;
    protected readonly IDataGridCommandScheduler _commandScheduler;
    protected List<T> _cash;
    
    public ActionCancel(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory, DataGridViewModel<T> dgVm, IDataGridCommandScheduler commandScheduler) : base(scheduler, context, commandFactory) {
        _dgVm = dgVm;
        _commandScheduler = commandScheduler;
    }

    public void SetCash() {
        _cash = _dgVm.Items.Select(item => (T)item.Clone()).ToList();
    }

    public override Task<BaseAction<TWindow, T>> PerformAsync(object? parameter = null) {
        _dgVm.Items.Clear();
        foreach (var item in _cash) {
            // Важно: если пользователь захочет отменить команду СНОВА, лучше опять сделать клонирование, чтобы кэш остался нетронутым.
            _dgVm.Items.Add((T)item.Clone());
        }

        _commandScheduler.RollbackDeferredChanges();

        return Task.FromResult<BaseAction<TWindow, T>>(this);
    }

    public override bool CanPerform() {
        return _commandScheduler.HaveDeferredChanges();
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
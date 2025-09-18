using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates.Core;

public abstract class BaseButtonsVm<TWindow, T, TContext> : ObservableObject, INotifyConditionalsChanged
    where TWindow : Window
    where T : class, IDbEntity
    where TContext : BaseAction<TWindow, T>
{
    protected readonly TContext _context;
    
    public BaseButtonsVm(TContext context) {
        _context = context;
        EventBus<IGlobalButtonEvent>.Subscribe(this);
    }
    
    public virtual void Dispose() {
        EventBus<IGlobalButtonEvent>.Unsubscribe(this);
    }

    public abstract void NotifyCanExecute();
}
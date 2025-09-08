using CommunityToolkit.Mvvm.ComponentModel;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates.Core;

public abstract class BaseButtonsVm<TContext> : ObservableObject, INotifyConditionalsChanged
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
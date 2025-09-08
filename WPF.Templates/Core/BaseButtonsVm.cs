using CommunityToolkit.Mvvm.ComponentModel;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates.Core;

public abstract class BaseButtonsVm<TContext> : ObservableObject, INotifyConditionalsChanged
{
    protected readonly TContext _condDesignGridVm;
    
    public BaseButtonsVm(TContext condDesignGridVm) {
        _condDesignGridVm = condDesignGridVm;
        EventBus<IGlobalButtonEvent>.Subscribe(this);
    }
    
    public virtual void Dispose() {
        EventBus<IGlobalButtonEvent>.Unsubscribe(this);
    }

    public abstract void NotifyCanExecute();
}
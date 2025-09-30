using CommunityToolkit.Mvvm.ComponentModel;
using Utils.EventBus;

namespace Utils.WPF.Buttons;

public abstract class BaseButtonVm : ObservableObject, INotifyConditionalsChanged
{
    protected BaseButtonVm() {
        EventBus<IGlobalButtonEvent>.Subscribe(this);
    }

    public Func<bool> CanExecuteFunc {get; set;}

    public virtual void Dispose() => EventBus<IGlobalButtonEvent>.Unsubscribe(this);
    
    public virtual bool CanClick() => CanExecuteFunc?.Invoke() ?? true;
    public abstract void NotifyCanExecute();
}

public abstract class BaseAsyncBButtonVm : BaseButtonVm
{
    public Func<object?, Task> ClickActionAsync { get; set; }
    public abstract Task OnClickAsync();
}

public abstract class BaseSyncButtonVm : BaseButtonVm
{
    public Action<object?> ClickAction { get; set; }
    public abstract void OnClick();
}


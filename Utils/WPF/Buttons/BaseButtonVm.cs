using CommunityToolkit.Mvvm.ComponentModel;
using Utils.EventBus;

namespace Utils.WPF.Buttons;

public abstract class BaseButtonVm : ObservableObject, INotifyConditionalsChanged
{
    protected readonly Func<bool> _canExecuteFunc;
    
    protected BaseButtonVm(Func<bool> canExecuteFunc) {
        _canExecuteFunc = canExecuteFunc;
        EventBus<IGlobalButtonEvent>.Subscribe(this);
    }

    public virtual void Dispose() => EventBus<IGlobalButtonEvent>.Unsubscribe(this);

    public Action<object?> ClickAction { get; set; }

    public abstract void OnClick();
    public virtual bool CanClick() => _canExecuteFunc();
    public abstract void NotifyCanExecute();
}


using CommunityToolkit.Mvvm.ComponentModel;
using Utils.EventBus;

namespace Utils.WPF.Buttons;

public abstract class BaseButtonVm : ObservableObject, INotifyConditionalsChanged /* : IGlobalButtonEvent */
{
    protected string _label;
    protected BaseButtonVm(string label  = "Label:") {
        Label = label;
        EventBus<IGlobalButtonEvent>.Subscribe(this);
    }
    
    public Func<bool> CanExecuteFunc {get; set;}

    public virtual string Label {
        get => _label;
        set {
            if (value == _label) return;
            _label = value;
            OnPropertyChanged();
        }
    }

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


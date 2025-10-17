namespace Utils.WPF.Buttons;


public abstract class BaseActionAsyncCompletion
{
    public BaseActionAsyncCompletion(BaseButtonAdvanced button) {
        button.ClickActionAsync = Perform;
        button.CanExecuteFunc = CanPerform;
    }
    public abstract Task Perform(TaskCompletionSource tcs);
    public abstract bool CanPerform();
}

public abstract class BaseButtonAdvanced : BaseButtonVm
{
    public Func<TaskCompletionSource, Task> ClickActionAsync { get; set; } = null!;
    protected bool _isEnabled = true;
    
    public virtual async Task OnClickAsync() {
        if (!_isEnabled)
            return;
        
        _isEnabled = false;
        
        var tsc = new TaskCompletionSource();
        await ClickActionAsync.Invoke(tsc);
        
        _isEnabled = true;
    }
}


public abstract class BaseActionAsyncCompletion<T>
{
    public BaseActionAsyncCompletion(BaseButtonAdvanced<T> button) {
        button.ClickActionAsync = Perform;
        button.CanExecuteFunc = CanPerform;
    }
    public abstract void Perform(TaskCompletionSource<T> tcs);
    public abstract bool CanPerform();
}

public abstract class BaseButtonAdvanced<T> : BaseButtonVm
{
    public Action<TaskCompletionSource<T>> ClickActionAsync { get; set; } = null!;
    protected bool _isEnabled = true;
    
    public virtual async Task OnClickAsync() {
        if (!_isEnabled)
            return;
        
        _isEnabled = false;
        
        var tsc = new TaskCompletionSource<T>();
        ClickActionAsync.Invoke(tsc);
        await tsc.Task;
        
        _isEnabled = true;
    }
}

public abstract class BaseActionAsyncAwaited : BaseAsyncAction 
{
    public BaseActionAsyncAwaited(BaseAsyncButtonVm but) {
        but.ClickActionAsync = PerformAsync;
        but.CanExecuteFunc = CanPerform;
    }
}

public abstract class BaseAsyncButtonVm : BaseButtonVm
{
    public Func<object?, Task> ClickActionAsync { get; set; } = null!;

    public virtual async Task OnClickAsync() {
        await ClickActionAsync?.Invoke(this)!;
    }
}
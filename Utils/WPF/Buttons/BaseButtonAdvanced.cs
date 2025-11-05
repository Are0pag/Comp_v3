namespace Utils.WPF.Buttons;

/// <summary>
/// Best Usage
/// </summary>
public abstract class BaseButtonAdvanced : BaseButtonVm
{
    public Func<TaskCompletionSource, Task> ClickActionAsync { get; set; } = null!;
    protected bool _isEnabled = true;
    
    public virtual async Task OnClickAsync() {
        if (!_isEnabled)
            return;
        
        _isEnabled = false;
        
        var tsc = new TaskCompletionSource();
        try {
            await ClickActionAsync.Invoke(tsc);
        }
        catch (NullReferenceException e) {
            Console.WriteLine($"{e.Message} in {GetType().Name}");
            throw;
        }
        
        _isEnabled = true;
    }
}

/// <summary>
/// Best Usage
/// </summary>
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
        NotifyCanExecute();
    }
}

/// <summary>
/// Best Usage
/// </summary>
public abstract class BaseActionAsyncCompletion
{
    protected readonly BaseButtonAdvanced _button;
    public BaseActionAsyncCompletion(BaseButtonAdvanced button) {
        button.ClickActionAsync = Perform;
        button.CanExecuteFunc = CanPerform;
        _button = button;
    }
    public abstract Task Perform(TaskCompletionSource tcs);
    public abstract bool CanPerform();
}

public abstract class BaseActionAsyncSelfWaiting : BaseActionAsyncCompletion
{
    protected TaskCompletionSource? _currentTcs;
    protected BaseActionAsyncSelfWaiting(BaseButtonAdvanced button) : base(button) {
    }
    
    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}

/// <summary>
/// Best Usage
/// </summary>
public abstract class BaseActionAsyncCompletion<T>
{
    public BaseActionAsyncCompletion(BaseButtonAdvanced<T> button) {
        button.ClickActionAsync = Perform;
        button.CanExecuteFunc = CanPerform;
    }
    public abstract void Perform(TaskCompletionSource<T> tcs);
    public abstract bool CanPerform();
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
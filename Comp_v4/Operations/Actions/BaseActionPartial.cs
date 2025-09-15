namespace WPF.Templates;

public abstract class BaseActionPartial
{
    protected readonly List<BaseAction> _actions;

    protected BaseActionPartial(List<BaseAction> actions) {
        _actions = actions;
    }
    
    public abstract Task<BaseAction> PerformAsync(object? parameter = null);

    public abstract bool CanPerform();
    
    public abstract Task CancelAsync(object? parameter = null);
}


namespace Utils.WPF;

public abstract class BaseAsyncAction
{
    public abstract Task PerformAsync(object? parameter);
    public abstract bool CanPerform();
    public abstract Task CancelAsync(object? parameter = null);
}
namespace Utils.WPF;

public abstract class BaseAction
{
    public abstract Task PerformAsync(object? parameter = null);
    public abstract bool CanPerform();
    public abstract Task CancelAsync(object? parameter = null);
}
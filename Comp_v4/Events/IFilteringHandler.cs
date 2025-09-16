namespace WPF.Templates.TableWindow.Events;

public interface IFilteringHandler : IGlobSubscriber, IDisposable
{
    object? OnNewItemCreating();
}
namespace WPF.Templates.TableWindow.Events;

public interface IFilteringHandler : IGlobSubscriber, IDisposable
{
    object? OnNewItemCreating();
}

public interface IFilteringInputHandler : IGlobSubscriber, IDisposable
{
    object? OnUserStartFiltering();
    
    object? OnUserEndFiltering();
}
namespace WPF.Templates.TableWindow.v1.Events;

public interface IFilteringHandler : IGlobSubscriber, IDisposable
{
    object? OnSourceCollectionStartEditing();
    object? OnSourceCollectionStopEditing();
}

public interface IFilteringInputHandler : IGlobSubscriber, IDisposable
{
    object? OnUserStartFiltering();
    
    object? OnUserEndFiltering();
}
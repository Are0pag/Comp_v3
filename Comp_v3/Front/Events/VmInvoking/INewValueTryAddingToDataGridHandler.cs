namespace Comp_v3.Front.Events;

public interface INewValueTryAddingToDataGridHandler : IVmGlobalSubscriber
{
    Task HandleNewValueAdded(object? newValue);
}
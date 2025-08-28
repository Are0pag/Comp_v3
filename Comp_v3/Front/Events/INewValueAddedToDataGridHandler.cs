namespace Comp_v3.Front.Events;

public interface INewValueAddedToDataGridHandler : IVmGlobalSubscriber
{
    void HandleNewValueAdded(object newValue);
}
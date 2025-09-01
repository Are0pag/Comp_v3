namespace Comp_v3.Front.Events;

public interface INewValueTryAddingToDataGridHandler : IVmGlobalSubscriber
{
    void HandleNewValueAdded(object newValue);
}

public interface ICancelNewItemAddingHandler : IUiGlobalSubscriber
{
    void HandleCancelNewItemAdding();
}
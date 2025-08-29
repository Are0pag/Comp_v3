namespace Comp_v3.Front.Events;

public interface ICellAddingToDataGridHandler : IUiGlobalSubscriber
{
    void HandleNewValueAdding();
}
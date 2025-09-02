namespace Comp_v3.Front.Events.ViewInvoking.GridItemsInteractions;

public interface ICellAddingToDataGridHandler : IUiGlobalSubscriber
{
    void HandleNewValueAdding();
}
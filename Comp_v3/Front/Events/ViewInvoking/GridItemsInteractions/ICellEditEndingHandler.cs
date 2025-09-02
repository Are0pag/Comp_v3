using System.Windows.Controls;

namespace Comp_v3.Front.Events.ViewInvoking.GridItemsInteractions;

public interface ICellEditEndingHandler : IUiGlobalSubscriber
{
    Task HandleCellEdit(object? sender, DataGridCellEditEndingEventArgs args);
}
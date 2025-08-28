using System.Windows.Controls;

namespace Comp_v3.Front.Events;

public interface ICellEditEndingHandler : IUiGlobalSubscriber
{
    void HandleCellEdit(object? sender, DataGridCellEditEndingEventArgs args);
}
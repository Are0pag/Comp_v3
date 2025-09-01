using System.Windows.Controls;

namespace Comp_v3.Front.Events;

public interface ICellEditEndingHandler : IUiGlobalSubscriber
{
    Task HandleCellEdit(object? sender, DataGridCellEditEndingEventArgs args);
}
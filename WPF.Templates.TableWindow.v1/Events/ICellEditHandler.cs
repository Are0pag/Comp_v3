using System.Windows.Controls;

namespace WPF.Templates.TableWindow.v1.Events;

public interface ICellEditHandler : IGlobSubscriber, IDisposable
{
    bool IsEnabled { get; }

    void SetAccessToHandleCellEvents(bool isEnable);
    
    Task OnEnding(object? sender, DataGridCellEditEndingEventArgs e);
    
    Task OnBeginning(object? sender, DataGridBeginningEditEventArgs e);
}
using System.Windows.Controls;

namespace WPF.Templates.TableWindow.Events;

public interface ICellEditHandler : IGlobSubscriber, IDisposable
{
    bool IsEnabled { get; set; }
    
    void OnEnding(object? sender, DataGridCellEditEndingEventArgs e);
    
    void OnBeginning(object? sender, DataGridBeginningEditEventArgs e);
}
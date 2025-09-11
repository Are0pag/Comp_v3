using System.Windows.Controls;

namespace WPF.Templates.TableWindow.Events;

public interface ICellEditHandler : IGlobSubscriber, IDisposable
{
    //bool IsEnabled { get; set; }
    
    Task OnEnding(object? sender, DataGridCellEditEndingEventArgs e);
    
    Task OnBeginning(object? sender, DataGridBeginningEditEventArgs e);
}
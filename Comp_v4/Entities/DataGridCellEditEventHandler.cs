using System.Windows.Controls;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates;

public class DataGridCellEditEventHandler : ICellEditHandler
{
    public DataGridCellEditEventHandler() {
        EventBus<IGlobSubscriber>.Subscribe(this);
    }

    public virtual void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }
    
    public bool IsEnabled { get; set; } = true;
    
    public void OnEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        if (!IsEnabled) return;
        
    }

    public void OnBeginning(object? sender, DataGridBeginningEditEventArgs e) {
        if (!IsEnabled) return;
    }
}
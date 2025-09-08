using System.Windows.Controls;

namespace WPF.Templates;

public abstract class BaseUiEventHandler
{
    public abstract void HandleEvent(object? sender, DataGridCellEditEndingEventArgs e);
}
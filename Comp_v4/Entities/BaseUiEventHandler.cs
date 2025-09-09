using System.Windows.Controls;

namespace WPF.Templates;

/* Хотя я бы сказал что они скорее провайдеры */
public abstract class BaseUiEventHandler
{
    public Action<object?, DataGridCellEditEndingEventArgs> EventWasRaised { get; set; }
    public virtual void HandleEvent(object? sender, DataGridCellEditEndingEventArgs e) => EventWasRaised?.Invoke(sender, e);
}

public class DataGridCellEditEndingEventHandler : BaseUiEventHandler
{
    public override void HandleEvent(object? sender, DataGridCellEditEndingEventArgs e) {
        throw new NotImplementedException();
    }
}

public class DataGridOnBeginningEditEventHandler : BaseUiEventHandler
{
    
}

public class WindowPreviewKeyDownEventHandler : BaseUiEventHandler
{
    
}
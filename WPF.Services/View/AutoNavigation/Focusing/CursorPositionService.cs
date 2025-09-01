using System.Windows;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public abstract class CursorPositionService<T> where T : FrameworkElement
{
    public abstract void FocusAndEditItem(T container, object item, Dispatcher dispatcher);
    public abstract void FocusItem(T container, object item);
    public virtual void FocusAndEditItem(T container, object item) => FocusAndEditItem(container, item, Dispatcher.CurrentDispatcher);
}
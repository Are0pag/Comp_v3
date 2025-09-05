using System.Windows;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public abstract class CursorPositionService<T> where T : FrameworkElement
{
    public abstract DataGridMemento FocusAndEditItem(T container, object item, Dispatcher dispatcher);
    public abstract DataGridMemento FocusItem(T container, object item);
    public virtual DataGridMemento FocusAndEditItem(T container, object item) => FocusAndEditItem(container, item, Dispatcher.CurrentDispatcher);
    public abstract DataGridMemento RememberCursorPos(T container);
}
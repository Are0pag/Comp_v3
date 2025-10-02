using System.Windows.Input;

namespace Templates.Common.Events.Input;

public interface IMouseDoubleClickHandler : IGlobalMouseSubscriber
{
    void OnMouseDoubleClick(object sender, MouseButtonEventArgs e);
}
using System.Windows.Input;

namespace WPF.Templates.TableWindow.Events;

public interface IPreviewKeyDownHandler : IGlobSubscriber, IDisposable
{
    Task OnPreviewKeyDown(object sender, KeyEventArgs e);
    Task OnPreviewMouseDown(object sender, MouseButtonEventArgs e);
}
using System.Windows.Input;

namespace WPF.Templates.TableWindow.Events;

public interface IPreviewKeyDownHandler : IGlobSubscriber, IDisposable
{
    void OnPreviewKeyDown(object sender, KeyEventArgs e);
}
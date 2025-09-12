using System.Windows.Input;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Entities;

public class TableCommandBinder : IPreviewKeyDownHandler
{
    public TableCommandBinder() {
        EventBus<IGlobSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public async Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.OemPlus:
                break;
            case Key.Add:
                break;
            case Key.Delete:
                break;
        }
    }

    public async Task OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }
}
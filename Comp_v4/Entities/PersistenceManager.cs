using System.Windows.Input;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Entities;

public class PersistenceManager : IPreviewKeyDownHandler
{
    protected readonly IModuleCommandScheduler _scheduler;
    
    public PersistenceManager(IModuleCommandScheduler scheduler) {
        _scheduler = scheduler;
        EventBus<IGlobSubscriber>.Subscribe(this);
    }
    public virtual void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public async Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.S when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                
                break;
        }
    }

    public Task OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        return Task.CompletedTask;
    }
}
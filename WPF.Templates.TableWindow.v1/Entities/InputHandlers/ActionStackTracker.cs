using System.Windows.Input;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Events;

namespace WPF.Templates.TableWindow.v1.Entities.InputHandlers;

public class ActionStackTracker : IPreviewKeyDownHandler
{
    protected readonly IDataGridCommandScheduler _scheduler;
    
    public ActionStackTracker(IDataGridCommandScheduler scheduler) {
        _scheduler = scheduler;
        EventBus<IGlobSubscriber>.Subscribe(this);
    }
    public virtual void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public async Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Z when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                try {
                    await _scheduler.UndoAsync();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                }
                break;
            
            case Key.Z when e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift):
            case Key.Y when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                await _scheduler.RedoAsync();
                break;
        }
    }

    public Task OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        return Task.CompletedTask;
    }
}
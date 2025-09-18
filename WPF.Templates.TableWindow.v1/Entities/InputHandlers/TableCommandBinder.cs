using System.Windows.Input;
using Utils.EventBus;
using WPF.Templates;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Entities;

public class TableCommandBinder : IPreviewKeyDownHandler
{
    protected readonly ActionStartAddingNewItem _actionStartAddingNewItem;
    protected readonly ActionDeleteItem _actionDeleteItem;
    
    public TableCommandBinder(ActionStartAddingNewItem actionStartAddingNewItem, ActionDeleteItem actionDeleteItem) {
        _actionStartAddingNewItem = actionStartAddingNewItem;
        _actionDeleteItem = actionDeleteItem;
        EventBus<IGlobSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public virtual async Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Add when _actionStartAddingNewItem.CanPerform():
            case Key.OemPlus when _actionStartAddingNewItem.CanPerform():
                await _actionStartAddingNewItem.PerformAsync();
                break;
            
            case Key.Delete when _actionDeleteItem.CanPerform():
                await _actionDeleteItem.PerformAsync();
                break;
        }
    }

    public Task OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        return Task.CompletedTask;
    }
}
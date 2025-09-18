using System.Windows.Input;
using WPF.Templates;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Entities;

public class TableCommandBinderFilteringCompatible : TableCommandBinder, IFilteringInputHandler
{
    protected bool _isGridEditingCommandByKeyEnabled = true;
    
    public TableCommandBinderFilteringCompatible(ActionStartAddingNewItem actionStartAddingNewItem, ActionDeleteItem actionDeleteItem) 
        : base(actionStartAddingNewItem, actionDeleteItem) {
    }

    public override Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        if (_isGridEditingCommandByKeyEnabled)
            return base.OnPreviewKeyDown(sender, e);
        return Task.CompletedTask;
    }

    public virtual object? OnUserStartFiltering() {
        _isGridEditingCommandByKeyEnabled = false;
        return null;
    }

    public virtual object? OnUserEndFiltering() {
        _isGridEditingCommandByKeyEnabled = true;
        return null;
    }
}
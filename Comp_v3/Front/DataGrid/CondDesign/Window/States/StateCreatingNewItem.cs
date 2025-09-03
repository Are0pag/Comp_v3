using Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

/// <summary>
/// То есть состояние UI когда создаётся новый пустой объект
/// </summary>
public class StateCreatingNewItem : StateWindow
{
    public StateCreatingNewItem(
        IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, 
        HeterochromicCommandScheduler<IDeferredCommand> scheduler, 
        CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) 
        
        : base(propertyValueRestoreService, scheduler, cursorPositionService) {
    }

    public override async Task OneNewValueAdded(CognDesignGridWindow window, object? newValue) {
        if (newValue is not ConditionalDesignation conditionalDesignation) 
            throw new ArgumentException("New value is not a conditional designation in CognDesignGridWindow");
        
        if (!_scheduler.IsInTransaction) 
            throw new Exception("The scheduler is not in a transaction");
        
        await _scheduler.ExecuteCommand(new FocusingCommand(window, _cursorPositionService, conditionalDesignation));
        _scheduler.CommitTransaction();
        
        window.StateProvider.SwitchStateWindow(window.StateProvider.StateWaitingToInputIntoNewItem, window);
    }
}
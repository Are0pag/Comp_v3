using System.Windows.Controls;
using Comp_v3.Front.DataGrid.CondDesign.Commands;
using Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.ViewInvoking.GridItemsInteractions;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Utils.EventBus;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public class StateEditableGrid : StateWindow
{
    public StateEditableGrid(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, HeterochromicCommandScheduler scheduler, CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) : base(propertyValueRestoreService, scheduler, cursorPositionService) {
    }


    public override void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        if (ActionContext.ActionContextType == ActionContextType.GoToEditingStateAfterUndoNewItemCreation)
            return;
        
        try {
            base.OnCellEditEnding(window, sender, e);
        }
        catch (InvalidInputException ex) { 
            return;
        }
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellEditEndingHandler>(h => h.HandleCellEdit(sender, e));
    }

    public override async Task OneNewValueAdded(CognDesignGridWindow window, object? newValue) {
        if (!_scheduler.IsInTransaction) 
            throw new Exception("The scheduler is not in a transaction");
        
        await _scheduler.ExecuteCommand(new FocusingCommand(window, _cursorPositionService, (ConditionalDesignation)newValue!));
        await _scheduler.ExecuteCommand(new ChangeTargetWindowStateCommand(window, 
                                                                           this,
                                                                           window.StateProvider.StateWaitingToInputIntoNewItem));
        _scheduler.CommitTransaction();
    }
}
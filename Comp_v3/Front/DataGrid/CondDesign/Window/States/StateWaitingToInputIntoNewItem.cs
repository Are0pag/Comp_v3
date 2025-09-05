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

public class StateWaitingToInputIntoNewItem : StateWindow
{
    public StateWaitingToInputIntoNewItem(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, HeterochromicCommandScheduler scheduler, CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) : base(propertyValueRestoreService, scheduler, cursorPositionService) {
    }

    public override void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        try {
            base.OnCellEditEnding(window, sender, e);
        }
        catch (InvalidInputException ex) { 
            EventBus<IUiGlobalSubscriber>.RaiseEvent<ICancelNewItemAddingHandler>(h => h.HandleCancelNewItemAdding());
            Continue(window);
            return;
        }
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellAddingToDataGridHandler>(h => h.HandleNewValueAdding());
        Continue(window);
    }

    private void Continue(CognDesignGridWindow window) {
        _scheduler.ExecuteCommand(new ChangeTargetWindowStateCommand(window, this, window.StateProvider.StateEditableGrid));
        if (_scheduler.IsInTransaction) // в случае, если отработал обработчик ICancelNewItemAddingHandler
            _scheduler.CommitTransaction();
    }
}
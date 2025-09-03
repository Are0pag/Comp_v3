using System.Windows.Controls;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.ViewInvoking.GridItemsInteractions;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;
using Infrastructure.Command.Heterochromic;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public class StateWaitingToInputIntoNewItem : StateWindow
{
    public StateWaitingToInputIntoNewItem(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, HeterochromicCommandScheduler<IDeferredCommand> scheduler, CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) : base(propertyValueRestoreService, scheduler, cursorPositionService) {
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

    private static void Continue(CognDesignGridWindow window) {
        window.StateProvider.SwitchStateWindow(window.StateProvider.StateEditableGrid, window);
    }
}
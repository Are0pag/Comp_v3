using System.Windows.Controls;
using Comp_v3.Front.DataGrid.CondDesign.Commands;
using Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;
using Comp_v3.Front.DataGrid.CondDesign.Transactions;
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
    public StateEditableGrid(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, 
                             HeterochromicCommandScheduler scheduler, 
                             CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) 
        : base(propertyValueRestoreService, scheduler, cursorPositionService) {
    }

    public override void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        try {
            base.OnCellEditEnding(window, sender, e);
        }
        catch (InvalidInputException ex) { 
            return;
        }
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellEditEndingHandler>(h => h.HandleCellEdit(sender, e));
    }

    public override async Task OneNewValueAdded(CognDesignGridWindow window, object? newValue) {
        await _scheduler.RegisterCommandInto<CreateNewRawAndFocusTransaction>(
                             new FocusingCommand(window, _cursorPositionService, (ConditionalDesignation)newValue!)
                         )
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<CreateNewRawAndFocusTransaction>(
                             new ChangeTargetWindowStateCommand(window, this, window.StateProvider.StateWaitingToInputIntoNewItem)
                         )
                        .ExecuteLastRegisteredAsync();

        _scheduler.CommitTransaction<CreateNewRawAndFocusTransaction>();
    }
}
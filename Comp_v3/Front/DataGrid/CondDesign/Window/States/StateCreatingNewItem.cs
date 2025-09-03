using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;
using Dg = System.Windows.Controls.DataGrid;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

/// <summary>
/// То есть состояние UI когда создаётся новый пустой объект
/// </summary>
public class StateCreatingNewItem : StateWindow
{
    protected readonly CursorPositionService<System.Windows.Controls.DataGrid> _cursorPositionService;
    public StateCreatingNewItem(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, 
                                CursorPositionService<Dg> cursorPositionService) 
                                    : base(propertyValueRestoreService) {
        _cursorPositionService = cursorPositionService;
    }

    public override void OneNewValueAdded(CognDesignGridWindow window, object? newValue) {
        if (newValue is not ConditionalDesignation conditionalDesignation) 
            throw new ArgumentException("New value is not a conditional designation in CognDesignGridWindow");
        
        _cursorPositionService.FocusAndEditItem(window.InfoDataGrid, conditionalDesignation);
        window.StateProvider.SwitchStateWindow(window.StateProvider.StateWaitingToInputIntoNewItem, window);
    }
}
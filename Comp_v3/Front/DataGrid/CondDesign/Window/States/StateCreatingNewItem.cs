using Comp.ModelData.TechnicalItems;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

/// <summary>
/// То есть состояние UI когда создаётся новый пустой объект
/// </summary>
public class StateCreatingNewItem : StateWindow
{
    private readonly CursorPositionService<System.Windows.Controls.DataGrid> _cursorPositionService;
    public StateCreatingNewItem(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, 
                                CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) 
                                    : base(propertyValueRestoreService) {
        _cursorPositionService = cursorPositionService;
    }

    public override void OneNewValueAdded(CognDesignGridWindow window, object newValue) {
        if (newValue is not ConditionalDesignation conditionalDesignation) 
            throw new ArgumentException("New value is not a conditional designation in CognDesignGridWindow");
        
        _cursorPositionService.FocusAndEditItem(window.InfoDataGrid, conditionalDesignation);
        window.StateProvider.SwitchStateWindow(window.StateProvider.StateWaitingToInputIntoNewItem, window);
    }
}
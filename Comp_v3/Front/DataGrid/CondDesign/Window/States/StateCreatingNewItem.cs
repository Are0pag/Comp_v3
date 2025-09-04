using Comp_v3.Front.DataGrid.CondDesign.Commands;
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
    protected ConditionalDesignation? _cashedValue;
    public StateCreatingNewItem(
        IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, 
        HeterochromicCommandScheduler scheduler, 
        CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) 
        
        : base(propertyValueRestoreService, scheduler, cursorPositionService) {
    }

    public override async Task Entry(CognDesignGridWindow window) {

    

        //window.StateProvider.SwitchStateWindow(window.StateProvider.StateWaitingToInputIntoNewItem, window);
    }
}
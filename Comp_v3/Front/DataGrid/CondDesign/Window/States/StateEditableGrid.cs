using System.Windows.Controls;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;
using WPF.Services.UserActionsHandling.InputText;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public class StateEditableGrid : StateWindow
{
    public StateEditableGrid(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService) : base(propertyValueRestoreService) {
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
}
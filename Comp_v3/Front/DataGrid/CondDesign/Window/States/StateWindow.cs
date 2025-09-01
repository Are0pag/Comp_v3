using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using Comp.ModelData.TechnicalItems;
using WPF.Services.UserActionsHandling.InputText;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public abstract class StateWindow 
{
    protected readonly IEditStateService<ConditionalDesignation> _editStateService;

    protected StateWindow(IEditStateService<ConditionalDesignation> editStateService) {
        _editStateService = editStateService;
    }

    public virtual void OnBeginningEdit(CognDesignGridWindow window, object? sender, DataGridBeginningEditEventArgs e) {
        if (e.Column == null || e.Row.Item is not ConditionalDesignation conditionalDesignation) return;
        _editStateService.BeginEdit(conditionalDesignation, e.Column.GetPropertyName());
    }

    public virtual void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        if (e.Row.Item is not ConditionalDesignation conditionalDesignation) 
            throw new InvalidInputException("Invalid input");

        if (Validate(conditionalDesignation)) return;

        _editStateService.RollbackEdit(conditionalDesignation);
        throw new InvalidInputException("Invalid input");
    }

    public virtual void Entry(CognDesignGridWindow window) { }

    /// <summary>
    /// INewValueTryAddingToDataGridHandler involication
    /// </summary>
    public virtual void OneNewValueAdded(CognDesignGridWindow window, object newValue) { }

    public virtual void Exit(CognDesignGridWindow window) { }

    protected virtual bool Validate(ConditionalDesignation item) {
        return item.Designation.Length >= 1; /* Наименование может быть пустым */
    }
    

}
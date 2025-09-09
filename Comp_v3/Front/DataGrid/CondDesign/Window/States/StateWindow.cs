using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public abstract class StateWindow
{
    protected readonly HeterochromicCommandScheduler _scheduler;
    protected readonly CursorPositionService<System.Windows.Controls.DataGrid> _cursorPositionService;
    protected readonly IPropertyValueRestoreService<ConditionalDesignation> _propertyValueRestoreService;

    protected StateWindow(IPropertyValueRestoreService<ConditionalDesignation> propertyValueRestoreService, 
                          HeterochromicCommandScheduler scheduler, 
                          CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService) {
        _propertyValueRestoreService = propertyValueRestoreService;
        _scheduler = scheduler;
        _cursorPositionService = cursorPositionService;
    }

    public virtual void OnBeginningEdit(CognDesignGridWindow window, object? sender, DataGridBeginningEditEventArgs e) {
        if (e.Column == null || e.Row.Item is not ConditionalDesignation conditionalDesignation) return;
        _propertyValueRestoreService.RememberValue(conditionalDesignation, e.Column.GetPropertyName());
    }

    /// <summary>
    /// Вызывается также при:
    /// <list type="bullet">
    ///     <item> После отмены действия (когда была создана пустая строка и поставлен курсор) </item>
    ///     <item>  </item>
    /// </list>
    /// </summary>
    public virtual void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        if (e.Row.Item is not ConditionalDesignation conditionalDesignation) throw new Exception("Can't edit the cell.");

        if (Validate(conditionalDesignation)) return;

        _propertyValueRestoreService.RollbackEdit(conditionalDesignation);
        throw new InvalidInputException("Invalid input");
    }

    public virtual async Task Entry(CognDesignGridWindow window) { }

    /// <summary>
    /// INewValueTryAddingToDataGridHandler involication
    /// </summary>
    public virtual async Task OneNewValueAdded(CognDesignGridWindow window, object? newValue) { }

    public virtual void Exit(CognDesignGridWindow window) { }

    protected virtual bool Validate(ConditionalDesignation item) {
        if (item == null || item.Designation == null) return false;
        return item.Designation.Length >= 1; /* Наименование может быть пустым */
    }
}
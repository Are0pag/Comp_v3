using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using Comp.ModelData.TechnicalItems;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public abstract class StateWindow
{
    protected string? _inputBackup;
    protected PropertyInfo? _editingProperty;

    public virtual void OnBeginningEdit(CognDesignGridWindow window, object? sender, DataGridBeginningEditEventArgs e) {
        var column = e.Column;
        var item = e.Row.Item;

        if (column == null || item == null) return; // Получаем имя свойства из привязки колонки
        var propertyName = GetPropertyNameFromColumn(column);

        // Получаем текущее значение свойства через рефлексию
        _editingProperty = item.GetType().GetProperty(propertyName);
        if (_editingProperty != null) {
            _inputBackup = _editingProperty.GetValue(item)?.ToString();
        }
    }

    public virtual void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) {
        if (e.Row.Item is not ConditionalDesignation conditionalDesignation) throw new InvalidInputException("Invalid input");
        if (Validate(conditionalDesignation)) return;
        _editingProperty?.SetValue(e.Row.Item, _inputBackup);
        throw new InvalidInputException("Invalid input");
    }

    public virtual void Entry(CognDesignGridWindow window) { }

    /// <summary>
    /// INewValueTryAddingToDataGridHandler involication
    /// </summary>
    public virtual void OneNewValueAdded(CognDesignGridWindow window, object newValue) { }

    public virtual void Exit(CognDesignGridWindow window) { }

    protected virtual bool Validate(ConditionalDesignation item) {
        return item.Designation.Length >= 1;
    }
    
    private string GetPropertyNameFromColumn(DataGridColumn column) {
        // Для привязанных колонок извлекаем имя свойства из привязки
        if (column is not DataGridBoundColumn boundColumn) return string.Empty;
        var binding = boundColumn.Binding as Binding;
        return binding?.Path?.Path ?? string.Empty;
    }
}
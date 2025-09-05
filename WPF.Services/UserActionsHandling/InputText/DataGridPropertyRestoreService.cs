using System.Reflection;

namespace WPF.Services.UserActionsHandling.InputText;

public class DataGridPropertyRestoreService<T> : IPropertyValueRestoreService<T>
{
    private string? _previousValue;
    private PropertyInfo? _currentEditedProperty;

    public void RememberValue(T item, string propertyName) {
        _previousValue = GetCurrentValue(item, propertyName);
    }

    public void RollbackEdit(T item) {
        if (_currentEditedProperty != null && _previousValue != null) 
            _currentEditedProperty.SetValue(item, _previousValue);
    }

    public string? GetPreviousValue() => _previousValue;
    public PropertyInfo? GetEditedProperty() => _currentEditedProperty;

    public string? GetCurrentValue(T item, string propertyName) {
        _currentEditedProperty = item.GetType().GetProperty(propertyName);
        return _currentEditedProperty != null ? _currentEditedProperty.GetValue(item)?.ToString() : null;
    }
}
using System.Reflection;

namespace WPF.Services.UserActionsHandling.InputText;

public class DataGridEditStateService<T> : IEditStateService<T>
{
    private string? _previousValue;
    private PropertyInfo? _currentEditedProperty;

    public void BeginEdit(T item, string propertyName) {
        _currentEditedProperty = item.GetType().GetProperty(propertyName);

        if (_currentEditedProperty != null) 
            _previousValue = _currentEditedProperty.GetValue(item)?.ToString();
    }

    public void RollbackEdit(T item) {
        if (_currentEditedProperty != null && _previousValue != null) 
            _currentEditedProperty.SetValue(item, _previousValue);
    }

    public string? GetPreviousValue() => _previousValue;
    public PropertyInfo? GetEditedProperty() => _currentEditedProperty;
}
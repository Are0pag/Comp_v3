using System.Reflection;

namespace Services.UserActionsHandling.Input;

public class EditStateService : IEditStateService
{
    protected string? _previousValue;
    protected PropertyInfo? _currentEditedProperty;

    public void BeginEdit<T>(T item, PropertyInfo property) {
        _currentEditedProperty = property;
        _previousValue = property.GetValue(item)?.ToString();
    }

    public void RollbackEdit<T>(T item, PropertyInfo property) {
        if (_currentEditedProperty != null && _previousValue != null) 
            property.SetValue(item, _previousValue);
    }

    public string? GetPreviousValue() => _previousValue;
}
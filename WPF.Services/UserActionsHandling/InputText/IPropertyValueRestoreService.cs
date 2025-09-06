using System.Reflection;

namespace WPF.Services.UserActionsHandling.InputText;

public interface IPropertyValueRestoreService<T>
{
    void RememberValue(T item, string propertyName);
    
    void RollbackEdit(T item);
    
    string? GetPreviousValue();
    
    PropertyInfo? GetEditedProperty();
    
    string? GetCurrentValue(T item, string propertyName);

    PropertyInfo? getCurrentPropertyInfo();
}
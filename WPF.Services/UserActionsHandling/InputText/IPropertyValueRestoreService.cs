using System.Reflection;

namespace WPF.Services.UserActionsHandling.InputText;

public interface IPropertyValueRestoreService<T>
{
    void BeginEdit(T item, string propertyName);
    void RollbackEdit(T item);
    string? GetPreviousValue();
    PropertyInfo? GetEditedProperty();
}
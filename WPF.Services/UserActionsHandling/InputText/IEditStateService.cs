using System.Reflection;

namespace WPF.Services.UserActionsHandling.InputText;

public interface IEditStateService
{
    void BeginEdit<T>(T item, PropertyInfo property);
    void RollbackEdit<T>(T item, PropertyInfo property);
    string? GetPreviousValue();
}
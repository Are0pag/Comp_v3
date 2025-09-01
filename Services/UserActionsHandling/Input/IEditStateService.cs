using System.Reflection;

namespace Services.UserActionsHandling.Input;

public interface IEditStateService
{
    void BeginEdit<T>(T item, PropertyInfo property);
    void RollbackEdit<T>(T item, PropertyInfo property);
    string? GetPreviousValue();
}
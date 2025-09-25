using System.Reflection;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

public static class ObjectStringCleaner
{
    public static void SetStringPropertiesToNull<T>(T obj) where T : class {
        var properties = obj.GetType().GetProperties(
                                                     BindingFlags.Public | BindingFlags.Instance
                                                    );

        foreach (var property in properties)
            if (property.PropertyType == typeof(string) && property.CanWrite)
                try {
                    property.SetValue(obj, null);
                }
                catch (Exception ex) {
                    Console.WriteLine($"Не удалось установить свойство {property.Name}: {ex.Message}");
                }
    }
}
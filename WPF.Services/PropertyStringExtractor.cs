using System.Reflection;
using System.Text;

namespace WPF.Services;

public class PropertyStringExtractor
{
    public static string ExtractStringProperties<T>(T obj) where T : class {
        if (obj == null)
            return null;

        var stringBuilder = new StringBuilder();

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
            if (property.PropertyType == typeof(string)) {
                var value = property.GetValue(obj) as string;
                stringBuilder.AppendLine($"{property.Name}: {value ?? "null"}");
            }
        return stringBuilder.ToString().TrimEnd();
    }
}
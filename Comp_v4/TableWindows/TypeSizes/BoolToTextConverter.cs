using System.Globalization;
using System.Windows.Data;

namespace Comp_v4.TableWindows.TypeSizes;

public class BoolToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is bool boolValue 
            ? (boolValue ? "Да" : "Нет") 
            : string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is string stringValue) {
            return stringValue.Equals("да", StringComparison.CurrentCultureIgnoreCase) || 
                   stringValue.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }
        return false;
    }
}
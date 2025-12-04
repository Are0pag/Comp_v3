using System.Globalization;
using System.Windows.Data;
using Comp.ModelData;

namespace Comp_v4.TableWindows.OrderPositions.Form;

public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is Enum enumValue)
            return enumValue.GetDescription();
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
using System;
using System.Globalization;
using System.Windows.Data;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Counterparties.Table;

public class CounterpartyTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string type) {
            return type switch
            {
                "Client"   => "Клиент",
                "Supplier" => "Поставщик",
                _                         => value.ToString()
            };
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing; // Для ComboBox обратная конвертация здесь не требуется
    }
}
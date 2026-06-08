using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Comp_v4.TableWindows.SupplierOrders.Table;

public class FilterEnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        // Если значение null (пункт сброса фильтра), возвращаем "Все"
        if (value == null) {
            return "Все";
        }

        // Получаем информацию о поле enum для поиска атрибута [Description]
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo != null) {
            // Ищем атрибут Description
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // Если атрибут найден, возвращаем его текст
            if (attributes.Length > 0) {
                return attributes[0].Description;
            }
        }

        // Если атрибута нет, возвращаем просто строковое имя enum (на всякий случай)
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        // В обратную сторону конвертер для фильтра не используется
        throw new NotImplementedException();
    }
}
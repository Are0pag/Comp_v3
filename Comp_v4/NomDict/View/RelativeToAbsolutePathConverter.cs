using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Comp_v4.NomDict.View;

public class RelativeToAbsolutePathConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string relativePath && !string.IsNullOrWhiteSpace(relativePath))
        {
            // Собираем полный путь к файлу на диске
            string absolutePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
                
            if (File.Exists(absolutePath))
            {
                return absolutePath;
            }
        }
        return null; // Если пути нет или файл отсутствует
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
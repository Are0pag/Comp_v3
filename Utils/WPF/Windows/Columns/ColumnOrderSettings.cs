using System.IO;
using System.Text.Json;
using System.Windows.Controls;

namespace Utils.WPF.Windows;

public class ColumnOrderSettings
{
    // Загрузка всех настроек порядка из файла "column_order.json"
    private static Dictionary<string, Dictionary<string, int>> LoadAllSettings()
    {
        string filePath = GetConfigFilePath();
        try
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(json)
                   ?? new Dictionary<string, Dictionary<string, int>>();
        }
        catch
        {
            return new Dictionary<string, Dictionary<string, int>>();
        }
    }

    // Сохранение текущего порядка (DisplayIndex) колонок конкретного окна
    public static void SaveColumnsOrder(DataGrid dataGrid, string windowKey)
    {
        var allSettings = LoadAllSettings();
        var windowSettings = new Dictionary<string, int>();

        foreach (var column in dataGrid.Columns)
        {
            string columnKey = GetColumnKey(column);
            if (!string.IsNullOrEmpty(columnKey))
            {
                windowSettings[columnKey] = column.DisplayIndex;
            }
        }

        allSettings[windowKey] = windowSettings;

        try
        {
            string json = JsonSerializer.Serialize(allSettings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(GetConfigFilePath(), json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save column order: {ex.Message}");
            throw;
        }
    }

    // Восстановление порядка (DisplayIndex) колонок
    public static void LoadColumnsOrder(DataGrid dataGrid, string windowKey)
    {
        var allSettings = LoadAllSettings();
        if (!allSettings.TryGetValue(windowKey, out var windowSettings))
        {
            return; // Настроек порядка для этого окна еще нет
        }

        // Сортируем пары настроек по возрастанию DisplayIndex, чтобы избежать конфликтов при восстановлении индексов
        var sortedSettings = windowSettings.OrderBy(x => x.Value).ToList();

        foreach (var setting in sortedSettings)
        {
            // Ищем колонку, соответствующую сохраненному ключу
            var targetColumn = dataGrid.Columns.FirstOrDefault(c => GetColumnKey(c) == setting.Key);
            
            // Проверяем корректность диапазона индекса перед установкой
            if (targetColumn != null && setting.Value >= 0 && setting.Value < dataGrid.Columns.Count)
            {
                targetColumn.DisplayIndex = setting.Value;
            }
        }
    }

    // Тот же хелпер для получения уникального идентификатора колонки, что и в вашем коде
    private static string GetColumnKey(DataGridColumn column)
    {
        if (!string.IsNullOrEmpty(column.SortMemberPath))
            return column.SortMemberPath;
        if (column.Header is string headerText)
            return headerText;
        return null;
    }

    private static string GetConfigFilePath() {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string companyFolder = Path.Combine(appData, WindowSettings.COMPANY_NAME);
        Directory.CreateDirectory(companyFolder);
        string filePath = Path.Combine(companyFolder, "column_orders.json");

        // Если файла нет, создаем пустой
        if (!File.Exists(filePath)) {
            // Сразу закрываем поток, чтобы файл не был заблокирован
            File.Create(filePath).Close();
        }

        return filePath;
    }
}

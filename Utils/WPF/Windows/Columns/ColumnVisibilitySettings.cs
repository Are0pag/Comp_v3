using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Utils.WPF.Windows;

public class ColumnVisibilitySettings
{
    private static Dictionary<string, Dictionary<string, bool>> LoadAllSettings() {
        string filePath = GetConfigFilePath();

        try {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(json)
                   ?? new Dictionary<string, Dictionary<string, bool>>();
        }
        catch {
            return new Dictionary<string, Dictionary<string, bool>>();
        }
    }

    // Сохранение видимости колонок конкретного окна
    public static void SaveColumnsVisibility(DataGrid dataGrid, string windowKey) {
        var allSettings = LoadAllSettings();
        var windowSettings = new Dictionary<string, bool>();

        foreach (var column in dataGrid.Columns) {
            string columnKey = GetColumnKey(column);
            if (!string.IsNullOrEmpty(columnKey)) {
                windowSettings[columnKey] = column.Visibility == Visibility.Visible;
            }
        }

        allSettings[windowKey] = windowSettings;

        try {
            string json = JsonSerializer.Serialize(allSettings, new JsonSerializerOptions { WriteIndented = true });
            File.ReadAllText(GetConfigFilePath()); // Проверка доступности перед записью
            File.WriteAllText(GetConfigFilePath(), json);
        }
        catch (Exception ex) {
            Console.Error.WriteLine($"Failed to save column visibility: {ex.Message}");
            throw;
        }
    }

    // Восстановление видимости колонок и обновление состояния ComboBox
    public static void LoadColumnsVisibility(DataGrid dataGrid, string windowKey, ComboBox comboBox) {
        var allSettings = LoadAllSettings();
        if (!allSettings.TryGetValue(windowKey, out var windowSettings)) {
            return; // Настроек для этого окна еще нет
        }

        foreach (var column in dataGrid.Columns) {
            string columnKey = GetColumnKey(column);
            if (!string.IsNullOrEmpty(columnKey) && windowSettings.TryGetValue(columnKey, out bool isVisible)) {
                column.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        // 2. ИСПРАВЛЕНО: Синхронизируем флажки (CheckBox) в ComboBox через Header колонок
        foreach (var item in comboBox.Items) {
            if (item is CheckBox checkBox) {
                // Ищем колонку, у которой Header совпадает с текстом (Content) на чекбоксе
                // Например: CheckBox.Content == "Наименование" и Column.Header == "Наименование"
                var targetColumn = dataGrid.Columns
                                           .FirstOrDefault(c => c.Header?.ToString() == checkBox.Content?.ToString());

                if (targetColumn != null) {
                    // Синхронизируем состояние флажка с реальной видимостью колонки
                    checkBox.IsChecked = targetColumn.Visibility == Visibility.Visible;
                }
            }
        }
    }

    // Вспомогательный метод для получения уникального идентификатора колонки
    private static string GetColumnKey(DataGridColumn column) {
        // Ищем имя, затем заголовок, затем путь сортировки
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
        string filePath = Path.Combine(companyFolder, "column_visibility.json");

        // Если файла нет, создаем пустой
        if (!File.Exists(filePath)) {
            // Сразу закрываем поток, чтобы файл не был заблокирован
            File.Create(filePath).Close();
        }

        return filePath;
    }
}
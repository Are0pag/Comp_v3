using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Interop;

public static class WindowSettings
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

    private const int SW_SHOWNORMAL = 1;
    private const string COMPANY_NAME = "AreopagComponent";
    private static readonly object FileLock = new();

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    private struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
    }

    private static string GetConfigFilePath() {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string companyFolder = Path.Combine(appData, COMPANY_NAME);
        Directory.CreateDirectory(companyFolder);
        return Path.Combine(companyFolder, "window_states.json");
    }

    public static void SavePlacement(Window window, string windowKey) {
        WindowInteropHelper helper = new WindowInteropHelper(window);
        if (helper.Handle == IntPtr.Zero)
            return;

        var placement = new WINDOWPLACEMENT();
        placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

        if (!GetWindowPlacement(helper.Handle, out placement))
            return;

        // Если координаты по какой-то причине нулевые, не сохраняем их
        if (placement.rcNormalPosition.Left == 0 && placement.rcNormalPosition.Right == 0)
            return;

        var options = new JsonSerializerOptions { IncludeFields = true };
        string placementJson = JsonSerializer.Serialize(placement, options);

        lock (FileLock) {
            var allSettings = LoadAllSettings();
            allSettings[windowKey] = placementJson;
            string finalJson = JsonSerializer.Serialize(allSettings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(GetConfigFilePath(), finalJson);
        }
    }

    public static void LoadPlacement(Window window, string windowKey) {
        try {
            var allSettings = LoadAllSettings();
            if (!allSettings.TryGetValue(windowKey, out string? placementJson) || string.IsNullOrWhiteSpace(placementJson))
                return; // Если настроек нет, НИЧЕГО не делаем. Окно откроется как обычно.

            var options = new JsonSerializerOptions { IncludeFields = true };
            WINDOWPLACEMENT placement = JsonSerializer.Deserialize<WINDOWPLACEMENT>(placementJson, options);
            placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            placement.flags = 0;

            // Если приложение закрыли свернутым, открываем его в нормальном режиме
            if (placement.showCmd == 2)
                placement.showCmd = SW_SHOWNORMAL;

            WindowInteropHelper helper = new WindowInteropHelper(window);
            if (helper.Handle != IntPtr.Zero) {
                SetWindowPlacement(helper.Handle, ref placement);
            }
        }
        catch {
            throw new FileNotFoundException("Cannot load settings from file: " + GetConfigFilePath());
        }
    }

    private static Dictionary<string, string> LoadAllSettings() {
        string filePath = GetConfigFilePath();
        lock (FileLock) {
            if (!File.Exists(filePath))
                return new Dictionary<string, string>();
            try {
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new Dictionary<string, string>();
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            catch {
                throw new FileNotFoundException("Cannot load settings from file: " + filePath);
            }
        }
    }
}
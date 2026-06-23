using System.ComponentModel;
using System.Windows;

namespace Utils.WPF.Windows;

public class PosWindowBase : Window
{
    public PosWindowBase() {
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
    }

    public void Dispose() {
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowPlaceSizeSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowPlaceSizeSettings.LoadPlacement(this, GetType().ToString());
}
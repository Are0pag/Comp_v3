using System.Runtime.InteropServices;
using System.Windows;

namespace Utils.WPF;

public enum WindowLocationOptions
{
    CenterScreen,
    CursorOnLeftTopCorner
}

public static class WindowLocator
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetCursorPos(out POINT lpPoint);
        
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT {
        public int X;
        public int Y;
    }

    public static T LocateBy<T>(T window, WindowLocationOptions options = WindowLocationOptions.CenterScreen)
        where T : Window {
        GetCursorPos(out POINT point);
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        
        switch (options) {
            case WindowLocationOptions.CenterScreen:
                break;
            case WindowLocationOptions.CursorOnLeftTopCorner:
                window.Left = point.X;
                window.Top = point.Y;
                break;
        }
        return window;
    }
}
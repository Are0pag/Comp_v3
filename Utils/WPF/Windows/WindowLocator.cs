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

    public static T LocateBy<T>(T window, WindowLocationOptions options = WindowLocationOptions.CursorOnLeftTopCorner)
        where T : Window 
    {
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

    public static void BindChildToParent(Window parent, Window child) {
        if (parent == null)
            throw new ArgumentNullException(nameof(parent));
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        // Устанавливаем владельца
        child.Owner = parent;

        // Логика удержания дочернего окна в границах родительского
        void KeepChildInside() {
            // 1. Контроль максимальных размеров
            if (child.Width > parent.ActualWidth)
                child.Width = parent.ActualWidth;
            if (child.Height > parent.ActualHeight)
                child.Height = parent.ActualHeight;

            // 2. Контроль по горизонтали (ось X)
            if (child.Left < parent.Left) {
                child.Left = parent.Left;
            }
            else if (child.Left + child.ActualWidth > parent.Left + parent.ActualWidth) {
                child.Left = parent.Left + parent.ActualWidth - child.ActualWidth;
            }

            // 3. Контроль по вертикали (ось Y)
            if (child.Top < parent.Top) {
                child.Top = parent.Top;
            }
            else if (child.Top + child.ActualHeight > parent.Top + parent.ActualHeight) {
                child.Top = parent.Top + parent.ActualHeight - child.ActualHeight;
            }
        }

        // Подписываемся на события обоих окон
        parent.LocationChanged += (s, e) => KeepChildInside();
        parent.SizeChanged += (s, e) => KeepChildInside();

        child.LocationChanged += (s, e) => KeepChildInside();
        child.SizeChanged += (s, e) => KeepChildInside();

        // Срабатывает при первом открытии для выравнивания
        child.ContentRendered += (s, e) => KeepChildInside();
    }

}



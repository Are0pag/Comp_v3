using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Utils.WPF;

public interface IWindowOrderLocator
{
    List<Window> Windows { get; }
    void MoveToFront<T>() where T : Window;
    void MoveToBack<T>() where T : Window;
    void RegisterWindow(Window window);
    void UnregisterWindow(Window window);
}

public class WindowOrderLocator : IWindowOrderLocator
{
    private readonly object _lock = new();
    private readonly List<Window> _windows = new();

    public void RegisterWindow(Window window) {
        if (window == null)
            throw new ArgumentNullException(nameof(window));

        lock (_lock) {
            if (!_windows.Contains(window)) {
                _windows.Add(window);
                window.Closed += OnWindowClosed;
            }
        }
    }

    public void UnregisterWindow(Window window) {
        if (window == null)
            return;

        lock (_lock) {
            window.Closed -= OnWindowClosed;
            _windows.Remove(window);
        }
    }

    public List<Window> Windows { get => _windows; }

    public void MoveToFront<T>() where T : Window {
        lock (_lock) {
            var window = _windows.OfType<T>().FirstOrDefault();
            if (window != null)
                BringToFront(window);
        }
    }

    public void MoveToBack<T>() where T : Window {
        lock (_lock) {
            var window = _windows.OfType<T>().FirstOrDefault();
            if (window != null)
                SendToBack(window);
        }
    }

    private void BringToFront(Window window) {
        if (window.WindowState == WindowState.Minimized)
            window.WindowState = WindowState.Normal;

        window.Activate();
        window.Topmost = true;
        window.Topmost = false;
        window.Focus();
    }

    private void SendToBack(Window window) {
        // Устанавливаем окно как самое нижнее в Z-order
        var hwnd = new WindowInteropHelper(window).Handle;
        if (hwnd != IntPtr.Zero)
            NativeMethods.SetWindowPos(
                                       hwnd,
                                       NativeMethods.HWND_BOTTOM,
                                       0, 0, 0, 0,
                                       NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOMOVE |
                                       NativeMethods.SWP_NOACTIVATE);
    }

    private void OnWindowClosed(object sender, EventArgs e) {
        if (sender is Window window)
            UnregisterWindow(window);
    }

    // Дополнительные методы для более гибкого управления
    public void MoveToFront(Window window) {
        lock (_lock) {
            if (_windows.Contains(window))
                BringToFront(window);
        }
    }

    public void MoveToBack(Window window) {
        lock (_lock) {
            if (_windows.Contains(window))
                SendToBack(window);
        }
    }

    public IEnumerable<Window> GetRegisteredWindows() {
        lock (_lock) {
            return _windows.ToList();
        }
    }
}

// Нативные методы для работы с оконным API
internal static class NativeMethods
{
    public const uint SWP_NOSIZE = 0x0001;
    public const uint SWP_NOMOVE = 0x0002;
    public const uint SWP_NOACTIVATE = 0x0010;
    public static readonly IntPtr HWND_BOTTOM = new(1);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
                                           int X, int Y, int cx, int cy, uint uFlags);
}


public static class WindowRegistrationBehavior
{
    public static readonly DependencyProperty OrderLocatorProperty =
        DependencyProperty.RegisterAttached("OrderLocator",
                                            typeof(IWindowOrderLocator),
                                            typeof(WindowRegistrationBehavior),
                                            new PropertyMetadata(null, OnOrderLocatorChanged));

    public static void SetOrderLocator(Window window, IWindowOrderLocator value) {
        window.SetValue(OrderLocatorProperty, value);
    }

    public static IWindowOrderLocator GetOrderLocator(Window window) {
        return (IWindowOrderLocator)window.GetValue(OrderLocatorProperty);
    }

    private static void OnOrderLocatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is not Window window)
            return;
        if (e.OldValue is IWindowOrderLocator oldLocator)
            oldLocator.UnregisterWindow(window);

        if (e.NewValue is IWindowOrderLocator newLocator)
            newLocator.RegisterWindow(window);
    }
}
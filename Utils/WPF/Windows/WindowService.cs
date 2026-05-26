using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

public static class WindowService
{
    private const int WM_WINDOWPOSCHANGING = 0x0046;

    [StructLayout(LayoutKind.Sequential)]
    private struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    public static void BindChildToParent(Window parent, Window child)
    {
        if (parent == null) throw new ArgumentNullException(nameof(parent));
        if (child == null) throw new ArgumentNullException(nameof(child));

        // 1. Базовая привязка
        child.Owner = parent;
        
        // 1.5. СКРЫВАЕМ ОКНО ИЗ ПАНЕЛИ ЗАДАЧ WINDOWS
        child.ShowInTaskbar = false;

        // 2. Отключаем дефолтное позиционирование WPF (будем выставлять координаты вручную)
        child.WindowStartupLocation = WindowStartupLocation.Manual;

        // 3. Задаем начальные ограничения размеров
        child.MaxWidth = parent.ActualWidth;
        child.MaxHeight = parent.ActualHeight;

        if (child.Width > parent.ActualWidth) child.Width = parent.ActualWidth;
        if (child.Height > parent.ActualHeight) child.Height = parent.ActualHeight;

        // 4. Изначальное позиционирование строго по центру родителя (до открытия)
        // Если у дочернего окна размеры заданы как Auto, используем Width/Height по умолчанию
        double childWidth = double.IsNaN(child.Width) ? 300 : child.Width; 
        double childHeight = double.IsNaN(child.Height) ? 200 : child.Height;

        child.Left = parent.Left + (parent.ActualWidth - childWidth) / 2;
        child.Top = parent.Top + (parent.ActualHeight - childHeight) / 2;

        // 5. Динамическое обновление ограничений размеров при изменении родителя
        SizeChangedEventHandler parentSizeChanged = (s, e) =>
        {
            child.MaxWidth = parent.ActualWidth;
            child.MaxHeight = parent.ActualHeight;

            // Если родитель сжался меньше, чем текущий размер дочернего окна — сжимаем дочернее
            if (child.Width > parent.ActualWidth) child.Width = parent.ActualWidth;
            if (child.Height > parent.ActualHeight) child.Height = parent.ActualHeight;
        };
        parent.SizeChanged += parentSizeChanged;

        // 6. Win32 Хук для плавного перемещения мышью
        child.SourceInitialized += (sender, args) =>
        {
            var windowInteropHelper = new WindowInteropHelper(child);
            var hwndSource = HwndSource.FromHwnd(windowInteropHelper.Handle);
            
            hwndSource?.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
            {
                if (msg == WM_WINDOWPOSCHANGING)
                {
                    WINDOWPOS wp = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

                    int parentLeft = (int)parent.Left;
                    int parentTop = (int)parent.Top;
                    int parentRight = parentLeft + (int)parent.ActualWidth;
                    int parentBottom = parentTop + (int)parent.ActualHeight;

                    if (wp.x < parentLeft) wp.x = parentLeft;
                    else if (wp.x + wp.cx > parentRight) wp.x = parentRight - wp.cx;

                    if (wp.y < parentTop) wp.y = parentTop;
                    else if (wp.y + wp.cy > parentBottom) wp.y = parentBottom - wp.cy;

                    Marshal.StructureToPtr(wp, lParam, true);
                }
                return IntPtr.Zero;
            });
        };

        // 7. Освобождение памяти
        child.Closed += (s, e) =>
        {
            parent.SizeChanged -= parentSizeChanged;
        };
    }
}

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

        // 1. Базовая привязка и скрытие с панели задач
        child.Owner = parent;
        child.ShowInTaskbar = false;

        // 2. Отключаем дефолтное позиционирование WPF
        child.WindowStartupLocation = WindowStartupLocation.Manual;

        // 3. Задаем начальные ограничения размеров
        child.MaxWidth = parent.ActualWidth;
        child.MaxHeight = parent.ActualHeight;

        if (child.Width > parent.ActualWidth) child.Width = parent.ActualWidth;
        if (child.Height > parent.ActualHeight) child.Height = parent.ActualHeight;

        // 4. Изначальное позиционирование строго по центру родителя
        double childWidth = double.IsNaN(child.Width) ? 300 : child.Width; 
        double childHeight = double.IsNaN(child.Height) ? 200 : child.Height;

        child.Left = parent.Left + (parent.ActualWidth - childWidth) / 2;
        child.Top = parent.Top + (parent.ActualHeight - childHeight) / 2;

        // Переменные для запоминания последней позиции родителя (нужны для расчета сдвига)
        double lastParentLeft = parent.Left;
        double lastParentTop = parent.Top;

        // 5. СИНХРОННОЕ ПЕРЕМЕЩЕНИЕ (Исправленный тип делегата: EventHandler)
        EventHandler parentLocationChanged = (s, e) =>
        {
            double deltaX = parent.Left - lastParentLeft;
            double deltaY = parent.Top - lastParentTop;

            child.Left += deltaX;
            child.Top += deltaY;

            lastParentLeft = parent.Left;
            lastParentTop = parent.Top;
        };
        parent.LocationChanged += parentLocationChanged;

        // Обновляем стартовую позицию родителя после его инициализации
        parent.SourceInitialized += (s, e) =>
        {
            lastParentLeft = parent.Left;
            lastParentTop = parent.Top;
        };

        // 6. Динамическое обновление ограничений размеров при изменении родителя
        SizeChangedEventHandler parentSizeChanged = (s, e) =>
        {
            child.MaxWidth = parent.ActualWidth;
            child.MaxHeight = parent.ActualHeight;

            if (child.Width > parent.ActualWidth) child.Width = parent.ActualWidth;
            if (child.Height > parent.ActualHeight) child.Height = parent.ActualHeight;
        };
        parent.SizeChanged += parentSizeChanged;

        // 7. Win32 Хук для плавного перемещения мышью (удержание внутри границ)
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
                    
                    lastParentLeft = parent.Left;
                    lastParentTop = parent.Top;
                }
                return IntPtr.Zero;
            });
        };

        // 8. Безопасное освобождение памяти от обоих событий
        child.Closed += (s, e) =>
        {
            parent.SizeChanged -= parentSizeChanged;
            parent.LocationChanged -= parentLocationChanged;
        };
    }

    public static void HideChildren(Window parent) {
        // Делаем копию списка, чтобы избежать ошибок изменения коллекции при скрытии
        var children = parent.OwnedWindows.Cast<Window>().ToArray();
        foreach (Window child in children) {
            HideChildren(child);
            child.Hide();
        }
    }
    
    public static void ShowChildren(Window parent) {
        var children = parent.OwnedWindows.Cast<Window>().ToArray();

        foreach (Window child in children) {
            child.Show();
            // Рекурсивно показываем вложенные окна
            ShowChildren(child);
        }
    }
}

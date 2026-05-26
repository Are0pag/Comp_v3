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

        child.Owner = parent;

        child.MaxWidth = parent.ActualWidth;
        child.MaxHeight = parent.ActualHeight;

        // ИСПОЛЬЗУЕМ ПРАВИЛЬНЫЙ ТИП: SizeChangedEventHandler
        SizeChangedEventHandler parentSizeChanged = (s, e) =>
        {
            child.MaxWidth = parent.ActualWidth;
            child.MaxHeight = parent.ActualHeight;
        };
        parent.SizeChanged += parentSizeChanged;

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

                    if (wp.x < parentLeft)
                    {
                        wp.x = parentLeft;
                    }
                    else if (wp.x + wp.cx > parentRight)
                    {
                        wp.x = parentRight - wp.cx;
                    }

                    if (wp.y < parentTop)
                    {
                        wp.y = parentTop;
                    }
                    else if (wp.y + wp.cy > parentBottom)
                    {
                        wp.y = parentBottom - wp.cy;
                    }

                    Marshal.StructureToPtr(wp, lParam, true);
                }
                return IntPtr.Zero;
            });
        };

        child.Closed += (s, e) =>
        {
            parent.SizeChanged -= parentSizeChanged;
        };
    }
}

using System.Windows;

namespace DI;

public static class WindowContextResolver
{
    public static Window ResolveWindow<T>(AreopagContainer container) where T : Window, IDisposable {
        var window = container.BeginScope<T>();
        window.Closed += (sender, args) => {
            container.ReleaseScope<T>();
        };
        window.Show();
        return window;
    }
}
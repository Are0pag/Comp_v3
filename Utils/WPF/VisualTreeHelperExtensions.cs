using System.Windows;
using System.Windows.Media;

namespace Utils.WPF;

public static class VisualTreeHelperExtensions
{
    public static T GetParentOfType<T>(this DependencyObject child) where T : DependencyObject {
        var parentObject = VisualTreeHelper.GetParent(child);

        while (parentObject != null) {
            if (parentObject is T parent)
                return parent;

            parentObject = VisualTreeHelper.GetParent(parentObject);
        }

        return null;
    }
}
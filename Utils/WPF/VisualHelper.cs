using System.Windows;

namespace Utils.WPF;

public static class VisualHelper
{
    public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject {
        for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++) {
            var child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
            if (child is T t)
                return t;
            var childOfChild = FindVisualChild<T>(child);
            if (childOfChild != null)
                return childOfChild;
        }

        return null;
    }
}
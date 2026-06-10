using System.Windows;

namespace Utils.WPF.Dialogs;

public static class DialogService
{
    public static bool ShowConfirmation(string title, string message) {
        MessageBoxResult result = MessageBox.Show(message, title, 
                                                  MessageBoxButton.YesNo, 
                                                  MessageBoxImage.Question);

        return result == MessageBoxResult.Yes;
    }
}
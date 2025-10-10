using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPF.UCL;

public partial class OpenImageWindow : Window
{
    public OpenImageWindow(string imagePath) {
        InitializeComponent();

        try {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();
            bitmap.Freeze();

            ImageViewer.Source = bitmap;
        }
        catch (Exception ex) {
            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            Close();
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) {
        Close();
    }

    protected override void OnKeyDown(KeyEventArgs e) {
        base.OnKeyDown(e);

        // Закрытие по Escape
        if (e.Key == Key.Escape) 
            Close();
    }
}
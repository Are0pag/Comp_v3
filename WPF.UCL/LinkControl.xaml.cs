using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.UCL;

/* Usage:   <ucl:LinkControl LinkUrl="https://yandex.ru/images/"/>
            <controls:LinkControl LinkUrl="{Binding CurrentItem.Link}"/>
 */
public partial class LinkControl : UserControl
{
    public static readonly DependencyProperty LinkUrlProperty =
        DependencyProperty.Register(nameof(LinkUrl),
                                    typeof(string),
                                    typeof(LinkControl),
                                    new PropertyMetadata(string.Empty));

    public LinkControl() {
        InitializeComponent();
    }

    public string LinkUrl {
        get => (string)GetValue(LinkUrlProperty);
        set => SetValue(LinkUrlProperty, value);
    }

    private void LinkTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        /*if (string.IsNullOrEmpty(LinkUrl) || (!LinkUrl.StartsWith("http://") && !LinkUrl.StartsWith("https://"))) 
            return;*/
        try {
            Process.Start(new ProcessStartInfo {
                FileName = LinkUrl,
                UseShellExecute = true
            });
        }
        catch {
            MessageBox.Show($"Не удалось открыть ссылку: {LinkUrl}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.UCL;

public partial class LinkFieldControl : UserControl
{
    public LinkFieldControl() {
        InitializeComponent();
    }

    public static readonly DependencyProperty FieldNameProperty =
        DependencyProperty.Register(
                                    nameof(FieldName),
                                    typeof(string),
                                    typeof(LinkFieldControl),
                                    new PropertyMetadata("Ссылка:")); // Значение по умолчанию

    public static readonly DependencyProperty LinkUrlProperty =
        DependencyProperty.Register(
                                    nameof(LinkUrl),
                                    typeof(string),
                                    typeof(LinkFieldControl),
                                    new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SetLinkCommandProperty =
        DependencyProperty.Register(
                                    nameof(SetLinkCommand),
                                    typeof(ICommand),
                                    typeof(LinkFieldControl),
                                    new PropertyMetadata(null));

    public string FieldName {
        get => (string)GetValue(FieldNameProperty);
        set => SetValue(FieldNameProperty, value);
    }

    public string LinkUrl {
        get => (string)GetValue(LinkUrlProperty);
        set => SetValue(LinkUrlProperty, value);
    }

    public ICommand SetLinkCommand {
        get => (ICommand)GetValue(SetLinkCommandProperty);
        set => SetValue(SetLinkCommandProperty, value);
    }

    private void LinkTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        try {
            Process.Start(new ProcessStartInfo {
                FileName = LinkUrl,
                UseShellExecute = true
            });
        }
        catch {
            MessageBox.Show($"Не удалось открыть ссылку: {LinkUrl}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
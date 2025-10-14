using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPF.UCL;

public partial class ImageFieldControl : UserControl
{
    public ImageFieldControl() {
        InitializeComponent();
    }

#region Dependency Properties

    // Изображение
    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(
                                    nameof(ImageSource),
                                    typeof(BitmapImage),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata(null));

    public BitmapImage ImageSource {
        get => (BitmapImage)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    // Подсказка для изображения
    public static readonly DependencyProperty ImageToolTipProperty =
        DependencyProperty.Register(
                                    nameof(ImageToolTip),
                                    typeof(string),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata("Изображение"));

    public string ImageToolTip {
        get => (string)GetValue(ImageToolTipProperty);
        set => SetValue(ImageToolTipProperty, value);
    }

    // Команды для кнопок
    public static readonly DependencyProperty Button1CommandProperty =
        DependencyProperty.Register(
                                    nameof(Button1Command),
                                    typeof(ICommand),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata(null));

    public ICommand Button1Command {
        get => (ICommand)GetValue(Button1CommandProperty);
        set => SetValue(Button1CommandProperty, value);
    }

    public static readonly DependencyProperty Button2CommandProperty =
        DependencyProperty.Register(
                                    nameof(Button2Command),
                                    typeof(ICommand),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata(null));

    public ICommand Button2Command {
        get => (ICommand)GetValue(Button2CommandProperty);
        set => SetValue(Button2CommandProperty, value);
    }

    public static readonly DependencyProperty Button3CommandProperty =
        DependencyProperty.Register(
                                    nameof(Button3Command),
                                    typeof(ICommand),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata(null));

    public ICommand Button3Command {
        get => (ICommand)GetValue(Button3CommandProperty);
        set => SetValue(Button3CommandProperty, value);
    }

    // Текст для кнопок
    public static readonly DependencyProperty Button1TextProperty =
        DependencyProperty.Register(
                                    nameof(Button1Text),
                                    typeof(string),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata("Кнопка 1"));

    public string Button1Text {
        get => (string)GetValue(Button1TextProperty);
        set => SetValue(Button1TextProperty, value);
    }

    public static readonly DependencyProperty Button2TextProperty =
        DependencyProperty.Register(
                                    nameof(Button2Text),
                                    typeof(string),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata("Кнопка 2"));

    public string Button2Text {
        get => (string)GetValue(Button2TextProperty);
        set => SetValue(Button2TextProperty, value);
    }

    public static readonly DependencyProperty Button3TextProperty =
        DependencyProperty.Register(
                                    nameof(Button3Text),
                                    typeof(string),
                                    typeof(ImageFieldControl),
                                    new PropertyMetadata("Кнопка 3"));

    public string Button3Text {
        get => (string)GetValue(Button3TextProperty);
        set => SetValue(Button3TextProperty, value);
    }

#endregion
}
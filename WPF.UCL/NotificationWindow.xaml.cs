using System.Windows;
using System.Windows.Media.Animation;

namespace WPF.UCL;

public partial class NotificationWindow : Window
{
    public NotificationWindow(string message)
    {
        InitializeComponent();

        // Установка сообщения
        MessageTextBlock.Text = message;

        // Позиционирование (например, в правом нижнем углу)
        WindowStartupLocation = WindowStartupLocation.Manual;
        Left = SystemParameters.WorkArea.Width - Width - 20;
        Top = SystemParameters.WorkArea.Height - Height - 20;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        // Анимация появления и исчезновения
        DoubleAnimation fadeInAnimation = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromSeconds(0.5)
        };

        DoubleAnimation fadeOutAnimation = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromSeconds(0.5)
        };

        // Настройка таймера автоматического закрытия
        fadeOutAnimation.Completed += (s, args) => Close();

        // Запуск анимаций
        BeginAnimation(OpacityProperty, fadeInAnimation);

        // Автоматическое закрытие через 3 секунды
        Dispatcher.BeginInvoke(new Action(() =>
        {
            System.Threading.Thread.Sleep(3000);
            Dispatcher.Invoke(() =>
            {
                BeginAnimation(OpacityProperty, fadeOutAnimation);
            });
        }));
    }

    // Метод для простого вызова
    public static void Show(string message)
    {
        var notification = new NotificationWindow(message);
        notification.Show();
    }
}
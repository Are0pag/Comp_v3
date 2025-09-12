using System.Windows;
using System.Windows.Media.Animation;

namespace WPF.UCL;

public partial class NotificationWindow : Window
{
    public NotificationWindow(string message) {
        InitializeComponent();

        MessageTextBlock.Text = message;

        // Получаем текущее активное окно
        Window activeWindow = Application.Current.Windows
                                         .OfType<Window>()
                                         .FirstOrDefault(w => w.IsActive);

        if (activeWindow != null)
        {
            // Позиционирование над активным окном
            Owner = activeWindow;
            WindowStartupLocation = WindowStartupLocation.Manual;

            // Расчет позиции по центру над активным окном
            Left = activeWindow.Left + (activeWindow.Width - Width) / 2;
            Top = activeWindow.Top - Height - 10; // Немного отступа сверху
        }
        else
        {
            // Резервный вариант - в правом нижнем углу экрана
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = SystemParameters.WorkArea.Width - Width - 20;
            Top = SystemParameters.WorkArea.Height - Height - 20;
        }
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
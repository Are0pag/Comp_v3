using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WPF.UCL;

/// <summary>
///     Создает и отображает всплывающее уведомление поверх активного окна.
/// </summary>
/// <param name="message">Текст уведомления для отображения</param>
/// <remarks>
///     Уведомление автоматически исчезает через 3 секунды.
///     Может быть вызвано из любого потока с использованием Dispatcher.
/// </remarks>
/// <example>
///     Пример использования в асинхронном контексте:
///     <code>
/// Task.Run(() => {
///     Application.Current.Dispatcher.Invoke(() => {
///         NotificationWindow.Show("Undo Stack = 0");
///     });
/// });
/// </code>
/// </example>
public partial class NotificationWindow : Window
{
    private readonly DispatcherTimer _closeTimer;

    public NotificationWindow(string message) {
        InitializeComponent();
        MessageTextBlock.Text = message;

        // Настройка окна
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
        Background = Brushes.Transparent;
        Topmost = true; // Важно: чтобы окно было поверх всех

        // Получаем текущее активное окно
        var activeWindow = Application.Current.Windows
                                      .OfType<Window>()
                                      .FirstOrDefault(w => w.IsActive);

        if (activeWindow != null) {
            // Позиционирование над активным окном
            Owner = activeWindow;
            WindowStartupLocation = WindowStartupLocation.Manual;

            // Расчет позиции по центру над активным окном
            Left = activeWindow.Left + (activeWindow.Width - Width) / 2;
            Top = activeWindow.Top - Height - 10;
        }
        else {
            // Резервный вариант - в правом нижнем углу экрана
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = SystemParameters.WorkArea.Width - Width - 20;
            Top = SystemParameters.WorkArea.Height - Height - 20;
        }

        // Таймер для автоматического закрытия
        _closeTimer = new DispatcherTimer();
        _closeTimer.Interval = TimeSpan.FromSeconds(3);
        _closeTimer.Tick += (s, e) => CloseWithAnimation();
    }

    protected override void OnSourceInitialized(EventArgs e) {
        base.OnSourceInitialized(e);
        StartAnimation();
    }

    private void StartAnimation() {
        // Анимация появления
        var fadeInAnimation = new DoubleAnimation {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromSeconds(0.5)
        };

        BeginAnimation(OpacityProperty, fadeInAnimation);

        // Запускаем таймер закрытия
        _closeTimer.Start();
    }

    private void CloseWithAnimation() {
        _closeTimer.Stop();

        // Анимация исчезновения
        var fadeOutAnimation = new DoubleAnimation {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromSeconds(0.5)
        };

        fadeOutAnimation.Completed += (s, args) => Close();
        BeginAnimation(OpacityProperty, fadeOutAnimation);
    }

    // Метод для простого вызова (теперь асинхронный)
    public static void Show(string message) {
        Application.Current.Dispatcher.Invoke(() => {
            var notification = new NotificationWindow(message);
            notification.Show();
        });
    }
}
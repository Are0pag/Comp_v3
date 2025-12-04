using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Comp.ModelData;

namespace Comp_v4.TableWindows.OrderPositions.Form;

public partial class OrderPositionForm : Window
{
    protected readonly OrderPosition _orderPosition;
    public OrderPositionForm(OrderPosition orderPosition) {
        InitializeComponent();
        _orderPosition = orderPosition;
        DataContext = _orderPosition;
    }

#region Невозможность прописать буквы и т.д. в поле

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
        // Разрешаем только цифры
        e.Handled = !IsTextAllowed(e.Text);
    }

    private static bool IsTextAllowed(string text) {
        // Регулярное выражение для проверки, что вводимый текст состоит только из цифр
        Regex regex = new Regex("[^0-9]+");
        return !regex.IsMatch(text);
    }

#endregion
}
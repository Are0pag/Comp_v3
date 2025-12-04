using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.OrderPositions.Form;

public partial class OrderPositionForm : Window, IRuntimeParamsResolver<OrderPosition>
{
    protected readonly ReceiveStatusEnumVm _receiveStatusEnumVm;
    protected readonly OrderPosition _orderPosition;
    public OrderPositionForm(OrderPosition orderPosition, ReceiveStatusEnumVm receiveStatusEnumVm) {
        InitializeComponent();
        _receiveStatusEnumVm = receiveStatusEnumVm;
        _orderPosition = orderPosition;
        
        DataContext = new OrderPositionVm(orderPosition);
        ReceiveStatusComboBox.DataContext = receiveStatusEnumVm;
        
        EventBus<IGlSubscriber>.Subscribe(this);
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

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPosition> container) {
        container.RuntimeParam = _orderPosition;
    }

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }


    private void ComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
        // Открываем выпадающий список по клику
        var comboBox = sender as ComboBox;
        if (comboBox != null) {
            comboBox.IsDropDownOpen = true;
            e.Handled = true; // Блокируем дальнейшую обработку
        }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        // Блокируем изменение выбора
        var comboBox = sender as ComboBox;
        if (comboBox != null && e.RemovedItems.Count > 0)
            // Возвращаем предыдущее выбранное значение
            comboBox.SelectedItem = e.RemovedItems[0];
    }
}
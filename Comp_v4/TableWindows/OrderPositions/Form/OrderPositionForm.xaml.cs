using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.OrderPositions.Form;

public partial class OrderPositionForm : Window, IRuntimeParamsResolver<OrderPosition>, IRuntimeParamsResolver<OrderPositionVm>
{
    protected readonly ReceiveStatusEnumVm _receiveStatusEnumVm;
    protected readonly OrderPosition _orderPosition;
    protected readonly SelectPositionButVm _selectPositionButVm; 
    protected readonly OrderPositionVm _orderPositionVm;
    protected readonly SaveOrderPositionButVm _saveOrderPositionButVm;
    public OrderPositionForm(OrderPosition orderPosition, ReceiveStatusEnumVm receiveStatusEnumVm, SelectPositionButVm selectPositionButVm, SaveOrderPositionButVm saveOrderPositionButVm) {
        InitializeComponent();
        _receiveStatusEnumVm = receiveStatusEnumVm;
        _selectPositionButVm = selectPositionButVm;
        _saveOrderPositionButVm = saveOrderPositionButVm;
        _orderPosition = orderPosition;

        _orderPositionVm = new OrderPositionVm(receiveStatusEnumVm, orderPosition);
        DataContext = _orderPositionVm;
        ReceiveStatusComboBox.DataContext = receiveStatusEnumVm;
        SelectPositionButton.DataContext = selectPositionButVm;
        SaveOrderPositionButton.DataContext = _saveOrderPositionButVm;
        
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPosition> container) => container.RuntimeParam = _orderPosition;

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPositionVm> container) => container.RuntimeParam = _orderPositionVm;

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
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
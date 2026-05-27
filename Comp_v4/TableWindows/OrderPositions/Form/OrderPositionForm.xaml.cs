using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.OrderPositions.Events;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.OrderPositions.Form;

public partial class OrderPositionForm : Window, IRuntimeParamsResolver<OrderPosition>, IRuntimeParamsResolver<OrderPositionVm>, IOrderPosSavingCommitHandler, IRuntimeParamsResolver<OrderPositionForm>
{
    protected readonly ReceiveStatusEnumVm _receiveStatusEnumVm;
    protected readonly OrderPosition _orderPosition;
    protected readonly SelectPositionButVm _selectPositionButVm; 
    protected readonly OrderPositionVm _orderPositionVm;
    protected readonly SaveOrderPositionButVm _saveOrderPositionButVm;
    public OrderPositionForm(OrderPosition orderPosition, ReceiveStatusEnumVm receiveStatusEnumVm, SelectPositionButVm selectPositionButVm, SaveOrderPositionButVm saveOrderPositionButVm) {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
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
        EventBus<IOrderPositionSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPosition> container) => container.RuntimeParam = _orderPosition;
    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPositionVm> container) => container.RuntimeParam = _orderPositionVm;
    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPositionForm> container) => container.RuntimeParam = this;

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
        EventBus<IOrderPositionSubscriber>.Unsubscribe(this);
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
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

    private void OpFormWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        _saveOrderPositionButVm.NotifyCanExecute();
    }

    Task IOrderPosSavingCommitHandler.OnSaveOp(TaskCompletionSource tcs, object? args) {
        Close();
        tcs.TrySetResult();
        return Task.CompletedTask;
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowSettings.LoadPlacement(this, GetType().ToString());
}
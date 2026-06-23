using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.PaymentOrders.Form;

public partial class PaymentOrderFormWindow : Window, IDisposable, IRuntimeParamsResolver<PaymentOrderFormWindow>
{
    private readonly PaymentOrder _paymentOrder;
    private readonly SavePaymentOrderButVm _savePaymentOrderButVm;
    private readonly CancelPaymentOrderButVm _cancelPaymentOrderButVm;
    public PaymentOrderFormWindow(SavePaymentOrderButVm savePaymentOrderButVm, PaymentOrder paymentOrder, CancelPaymentOrderButVm cancelPaymentOrderButVm) {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        _savePaymentOrderButVm = savePaymentOrderButVm;
        _paymentOrder = paymentOrder;
        _cancelPaymentOrderButVm = cancelPaymentOrderButVm;

        DataContext = paymentOrder;
        CorrespondingSoReadonlyGroupBox.DataContext = paymentOrder.Order;
        
        SaveButton.DataContext = _savePaymentOrderButVm;
        CancelButton.DataContext = _cancelPaymentOrderButVm;
        EventBus<IGlSubscriber>.Subscribe(this);

        Closed += (_, __) => {
            Dispose();
        };
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<PaymentOrderFormWindow> container) {
        if (!IsVisible) return;
        container.RuntimeParam = this;
    }

    public void Dispose() {
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowPlaceSizeSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowPlaceSizeSettings.LoadPlacement(this, GetType().ToString());

    private async void PaymentOrderFormWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Escape:
                await _cancelPaymentOrderButVm.OnClickAsync();
                break;
        }
    }
}
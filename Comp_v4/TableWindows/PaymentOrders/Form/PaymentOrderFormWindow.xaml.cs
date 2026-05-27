using System.ComponentModel;
using System.Windows;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp.ModelData;

namespace Comp_v4.TableWindows.PaymentOrders.Form;

public partial class PaymentOrderFormWindow : Window, IDisposable
{
    private readonly PaymentOrder _paymentOrder;
    private readonly SavePaymentOrderButVm _savePaymentOrderButVm;
    public PaymentOrderFormWindow(SavePaymentOrderButVm savePaymentOrderButVm, PaymentOrder paymentOrder) {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        _savePaymentOrderButVm = savePaymentOrderButVm;
        _paymentOrder = paymentOrder;
        
        DataContext = paymentOrder;
        SaveButton.DataContext = _savePaymentOrderButVm;
    }

    public void Dispose() {
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowSettings.LoadPlacement(this, GetType().ToString());
}
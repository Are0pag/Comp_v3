using System.Windows;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp.ModelData;

namespace Comp_v4.TableWindows.PaymentOrders.Form;

public partial class PaymentOrderFormWindow : Window
{
    private readonly PaymentOrder _paymentOrder;
    private readonly SavePaymentOrderButVm _savePaymentOrderButVm;
    public PaymentOrderFormWindow(SavePaymentOrderButVm savePaymentOrderButVm, PaymentOrder paymentOrder) {
        InitializeComponent();
        _savePaymentOrderButVm = savePaymentOrderButVm;
        _paymentOrder = paymentOrder;
        
        DataContext = paymentOrder;
        SaveButton.DataContext = _savePaymentOrderButVm;
    }
}
using System.Windows;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Form;

public partial class SupplierOrderFormWindow : Window, IDisposable
{
    public SupplierOrderFormWindow(SupplierOrder supplierOrder, SaveButVm saveButVm, CounterpartySelectButVm counterpartySelectButVm) {
        InitializeComponent();
        DataContext = supplierOrder;
        
        VatStatusComboBox.ItemsSource = Enum.GetValues(typeof(VatStatus)).Cast<VatStatus>();
        OrderStatusComboBox.ItemsSource = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
        
        SaveButton.DataContext = saveButVm;
        CounterpartySelectButton.DataContext = counterpartySelectButVm;
    }

    public void Dispose() {
        
    }
}
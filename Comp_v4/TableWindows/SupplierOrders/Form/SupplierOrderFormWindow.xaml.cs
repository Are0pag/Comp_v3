using System.Windows;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Form;

public partial class SupplierOrderFormWindow : Window, IDisposable
{
    public SupplierOrderFormWindow(SupplierOrder supplierOrder, SaveFormButVm saveButVm, CounterpartySelectButVm counterpartySelectButVm, 
                                   OrderStatusEnumsVm orderStatusEnumsVm, VatStatusEnumVm vatStatusEnumsVm) {
        InitializeComponent();
        DataContext = supplierOrder;

        VatStatusComboBox.DataContext = vatStatusEnumsVm;
        OrderStatusComboBox.DataContext = orderStatusEnumsVm;
        
        SaveButton.DataContext = saveButVm;
        CounterpartySelectButton.DataContext = counterpartySelectButVm;
    }

    public void Dispose() {
        Console.WriteLine($"{nameof(SupplierOrderFormWindow)} disposed");
    }
}
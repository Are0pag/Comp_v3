using System.Windows;
using System.Windows.Input;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form;

public partial class SupplierOrderFormWindow : Window, IDisposable
{
    public SupplierOrderFormWindow(SupplierOrder supplierOrder, 
                                   
                                   SaveFormButVm saveButVm, 
                                   CounterpartySelectButVm counterpartySelectButVm, 
                                   ResetOrderDateButVm resetOrderDateButVm,
                                   ResetDeliveryDateButVm resetDeliveryDateButVm,
                                   
                                   ContractLinkFieldVm contractLinkFieldVm,
                                   InvoiceLinkFieldVm invoiceLinkFieldVm,
                                   
                                   OrderStatusEnumsVm orderStatusEnumsVm, 
                                   VatStatusEnumVm vatStatusEnumsVm) {
        InitializeComponent();
        DataContext = supplierOrder;

        VatStatusComboBox.DataContext = vatStatusEnumsVm;
        OrderStatusComboBox.DataContext = orderStatusEnumsVm;
        
        SaveButton.DataContext = saveButVm;
        CounterpartySelectButton.DataContext = counterpartySelectButVm;
        
        OrderDateButton.DataContext = resetOrderDateButVm;
        DeliveryDateButton.DataContext = resetDeliveryDateButVm;
        
        ContractLinkFieldControl.DataContext = contractLinkFieldVm;
        InvoiceFilePathLinkFieldControl.DataContext = invoiceLinkFieldVm;
    }

    public void Dispose() {
        Console.WriteLine($"{nameof(SupplierOrderFormWindow)} disposed");
    }

    private void SupplierOrderFormWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        try {
            EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}
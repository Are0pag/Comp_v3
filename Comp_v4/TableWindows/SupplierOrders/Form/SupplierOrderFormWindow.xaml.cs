using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form;

public partial class SupplierOrderFormWindow : Window, IDisposable, IRuntimeParamsResolver<SupplierOrderFormWindow>
{
    protected readonly CounterpartySelectButVm _counterpartySelectButVm;
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
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        DataContext = supplierOrder;

        VatStatusComboBox.DataContext = vatStatusEnumsVm;
        OrderStatusComboBox.DataContext = orderStatusEnumsVm;
        
        SaveButton.DataContext = saveButVm;
        CounterpartySelectButton.DataContext = counterpartySelectButVm;
        
        OrderDateButton.DataContext = resetOrderDateButVm;
        DeliveryDateButton.DataContext = resetDeliveryDateButVm;
        
        ContractLinkFieldControl.DataContext = contractLinkFieldVm;
        InvoiceFilePathLinkFieldControl.DataContext = invoiceLinkFieldVm;
        
        _counterpartySelectButVm = counterpartySelectButVm;
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    private void SupplierOrderFormWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        _counterpartySelectButVm.NotifyCanExecute();
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<SupplierOrderFormWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowSettings.LoadPlacement(this, GetType().ToString());
}
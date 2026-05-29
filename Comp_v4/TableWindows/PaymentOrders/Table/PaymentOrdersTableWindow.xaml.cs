using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Utils.WPF.Windows;

namespace Comp_v4.TableWindows.PaymentOrders.Table;

public partial class PaymentOrdersTableWindow : TableWindowBase, IDisposable
{
    private readonly PaymentOrdersGridVm _gridVm;
    private readonly AddPaymentOrderButVm _addPaymentOrderButVm;
    private readonly EditPaymentOrderButVm _editPaymentOrderButVm;
    private readonly DeletePaymentOrderButVm _deletePaymentOrderButVm;
    
    public PaymentOrdersTableWindow(AddPaymentOrderButVm addPaymentOrderButVm, EditPaymentOrderButVm editPaymentOrderButVm, DeletePaymentOrderButVm deletePaymentOrderButVm, PaymentOrdersGridVm gridVm) {
        InitializeComponent();
        _gridVm = gridVm;
        _addPaymentOrderButVm = addPaymentOrderButVm;
        _editPaymentOrderButVm = editPaymentOrderButVm;
        _deletePaymentOrderButVm = deletePaymentOrderButVm;

        DataGrid.DataContext = _gridVm;
        AddButton.DataContext = _addPaymentOrderButVm;
        EditButton.DataContext = _editPaymentOrderButVm;
        DeleteButton.DataContext = _deletePaymentOrderButVm;
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        _addPaymentOrderButVm.NotifyCanExecute();
        _editPaymentOrderButVm.NotifyCanExecute();
        _deletePaymentOrderButVm.NotifyCanExecute();
    }

    private void Window_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        
    }
    
}
using System.Windows;
using System.Windows.Input;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;

namespace Comp_v4.TableWindows.PaymentOrders.Table;

public partial class PaymentOrdersTableWindow : Window
{
    private readonly PaymentOrdersGridVm _gridVm;
    private readonly AddPaymentOrderButVm _addPaymentOrderButVm;
    private readonly EditPaymentOrderButVm _editPaymentOrderButVm;
    private readonly DeletePaymentOrderButVm _deletePaymentOrderButVm;
    private readonly SavePaymentOrderButVm _savePaymentOrderButVm;
    
    public PaymentOrdersTableWindow(AddPaymentOrderButVm addPaymentOrderButVm, EditPaymentOrderButVm editPaymentOrderButVm, DeletePaymentOrderButVm deletePaymentOrderButVm, SavePaymentOrderButVm savePaymentOrderButVm, PaymentOrdersGridVm gridVm) {
        InitializeComponent();
        _gridVm = gridVm;
        _addPaymentOrderButVm = addPaymentOrderButVm;
        _editPaymentOrderButVm = editPaymentOrderButVm;
        _deletePaymentOrderButVm = deletePaymentOrderButVm;
        _savePaymentOrderButVm = savePaymentOrderButVm;

        DataGrid.DataContext = _gridVm;
        AddButton.DataContext = _addPaymentOrderButVm;
        EditButton.DataContext = _editPaymentOrderButVm;
        DeleteButton.DataContext = _deletePaymentOrderButVm;
        SaveButton.DataContext = _savePaymentOrderButVm;
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        
    }

    private void Window_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        
    }
}
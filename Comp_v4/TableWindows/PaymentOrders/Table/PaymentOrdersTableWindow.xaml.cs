using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Utils.WPF.Windows;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.PaymentOrders.Table;

public partial class PaymentOrdersTableWindow : TableWindowBase, IDisposable
{
    private readonly PaymentOrdersGridVm _gridVm;
    private readonly AddPaymentOrderButVm _addPaymentOrderButVm;
    private readonly EditPaymentOrderButVm _editPaymentOrderButVm;
    private readonly DeletePaymentOrderButVm _deletePaymentOrderButVm;
    
    public PaymentOrdersTableWindow(AddPaymentOrderButVm addPaymentOrderButVm, EditPaymentOrderButVm editPaymentOrderButVm, 
                                    DeletePaymentOrderButVm deletePaymentOrderButVm, PaymentOrdersGridVm gridVm) {
        InitializeComponent();
        _gridVm = gridVm;
        _addPaymentOrderButVm = addPaymentOrderButVm;
        _editPaymentOrderButVm = editPaymentOrderButVm;
        _deletePaymentOrderButVm = deletePaymentOrderButVm;

        DataGrid.DataContext = _gridVm;
        AddButton.DataContext = _addPaymentOrderButVm;
        EditButton.DataContext = _editPaymentOrderButVm;
        DeleteButton.DataContext = _deletePaymentOrderButVm;
        
        InfoDataGridContextMenuAddNewItemCommand.DataContext = addPaymentOrderButVm;
        InfoDataGridContextMenuDeleteItemCommand.DataContext = deletePaymentOrderButVm;
        
        var filtersVm = new FiltersVmBase();
        gridVm.FiltersVm = filtersVm;
        FilterTextBox.DataContext = filtersVm;
        IgnoreCaseCheckBox.DataContext = filtersVm;
        
        Loaded += (_, _) => {
            _ = gridVm.InitFilteringCollection();
        };
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        Notify();
    }

    private void Window_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        
    }

    private void DataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
        Notify();
    }

    private void PaymentOrdersTableWindow_OnContentRendered(object? sender, EventArgs e) {
        Notify();
    }

    private void Notify() {
        _addPaymentOrderButVm.NotifyCanExecute();
        _editPaymentOrderButVm.NotifyCanExecute();
        _deletePaymentOrderButVm.NotifyCanExecute();
    }
}
using System.Windows;
using System.Windows.Input;

namespace Comp_v4.TableWindows.PaymentOrders.Table;

public partial class PaymentOrdersTableWindow : Window
{
    public PaymentOrdersTableWindow() {
        InitializeComponent();
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        
    }

    private void Window_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        
    }
}
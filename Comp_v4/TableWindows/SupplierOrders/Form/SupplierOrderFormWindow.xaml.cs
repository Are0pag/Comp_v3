using System.Windows;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Form;

public partial class SupplierOrderFormWindow : Window
{
    public SupplierOrderFormWindow() {
        InitializeComponent();
        VatStatusComboBox.ItemsSource = Enum.GetValues(typeof(VatStatus)).Cast<VatStatus>();
        OrderStatusComboBox.ItemsSource = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
    }
}
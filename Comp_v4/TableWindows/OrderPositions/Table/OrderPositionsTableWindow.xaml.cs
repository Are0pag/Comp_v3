using System.Windows;
using System.Windows.Input;
using Comp.ModelData;

namespace Comp_v4.TableWindows.OrderPositions.Table;

public partial class OrderPositionsTableWindow : Window
{
    private readonly OrderPosition _orderPosition;
    
    public OrderPositionsTableWindow(OrderPosition orderPosition) {
        InitializeComponent();
        _orderPosition = orderPosition;
        DataContext = orderPosition;
        
    }

    private void SupplierOrderTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        throw new NotImplementedException();
    }

    private void SupplierOrderTableWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }
}
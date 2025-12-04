using System.Windows;
using System.Windows.Input;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp.ModelData;

namespace Comp_v4.TableWindows.OrderPositions.Table;

public partial class OrderPositionsTableWindow : Window
{
    private readonly OpDataGridVm _opDataGridVm;
    
    public OrderPositionsTableWindow(OpDataGridVm opDataGridVm) {
        InitializeComponent();
        _opDataGridVm = opDataGridVm;
        DataGrid.DataContext = opDataGridVm;
        
    }

    private void SupplierOrderTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        
    }

    private void SupplierOrderTableWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        
    }
}
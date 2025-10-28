using System.Windows;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;

namespace Comp_v4.TableWindows.SupplierOrders.Table;

public partial class SupplierOrderTableWindow : Window, IDisposable
{
    public SupplierOrderTableWindow(SoDataGridVm dataGridVm, AddSoButVm addButVm) {
        InitializeComponent();
        DataGrid.DataContext = dataGridVm;
        AddButton.DataContext = addButVm;
    }

    public void Dispose() {
        //Console.WriteLine($"{nameof(SupplierOrderTableWindow)} disposed");
    }
}
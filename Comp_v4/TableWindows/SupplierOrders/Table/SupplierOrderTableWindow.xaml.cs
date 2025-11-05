using System.Windows;
using System.Windows.Input;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;

namespace Comp_v4.TableWindows.SupplierOrders.Table;

public partial class SupplierOrderTableWindow : Window, IDisposable, IReloadable
{
    public SupplierOrderTableWindow(SoDataGridVm dataGridVm, AddSoButVm addButVm, EditSoButVm editButVm) {
        InitializeComponent();
        DataGrid.DataContext = dataGridVm;
        AddButton.DataContext = addButVm;
        EditButton.DataContext = editButVm;
    }

    public void Dispose() {
        //Console.WriteLine($"{nameof(SupplierOrderTableWindow)} disposed");
    }

    public Func<Task> OnReload { get; set; }

    private void SupplierOrderTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.K:
                OnReload?.Invoke();
                break;
        }
    }
}
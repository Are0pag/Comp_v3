using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Templates.Common;

namespace Comp_v4.TableWindows.SupplierOrders.Table;

public partial class SupplierOrderTableWindow : Window, IDisposable, IReloadable
{
    protected readonly EditSoButVm _editSoButVm;
    public SupplierOrderTableWindow(SoDataGridVm dataGridVm, AddSoButVm addButVm, EditSoButVm editButVm) {
        InitializeComponent();
        DataGrid.DataContext = dataGridVm;
        AddButton.DataContext = addButVm;
        EditButton.DataContext = editButVm;
        
        _editSoButVm = editButVm;
    }

    public void Dispose() {
        //Console.WriteLine($"{nameof(SupplierOrderTableWindow)} disposed");
    }

    public Func<Task> OnReload { get; set; }

    private void SupplierOrderTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
        #if DEBUG
            case Key.K:
                OnReload?.Invoke();
                break;
        #endif
        }
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        _ = HandleDoubleClick(sender, e);
    }
    
    private async Task HandleDoubleClick(object sender, MouseButtonEventArgs e) {
        // Ждем один цикл диспетчеризации
        await Application.Current.Dispatcher.InvokeAsync(() => {
            if (DataGrid.SelectedItem is null || !_editSoButVm.IsEnabled || !_editSoButVm.CanClick())
                return;
            _editSoButVm.OnClickAsync();
        }, DispatcherPriority.Background);
    }

    private void SupplierOrderTableWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        _editSoButVm.NotifyCanExecute();
    }
}
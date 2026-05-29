using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Comp_v4._Installers;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Templates.Common;
using Utils.EventBus;
using Utils.WPF.Windows;

namespace Comp_v4.TableWindows.SupplierOrders.Table;

public partial class SupplierOrderTableWindow : TableWindowBase, IDisposable, IReloadable, IRuntimeParamsResolver<SupplierOrderTableWindow>
{
    protected readonly EditSoButVm _editSoButVm;
    protected readonly OpenOrderPositionsButVm _positionsBut;
    protected readonly OpenPaymentOrdersButVm _paymentOrdersBut;
    protected readonly DeleteSoButVm _deleteSoButVm;
    
    public SupplierOrderTableWindow(SoDataGridVm dataGridVm, 
                                    AddSoButVm addButVm, EditSoButVm editButVm, DeleteSoButVm deleteSoButVm, 
                                    OpenOrderPositionsButVm positionsBut, OpenPaymentOrdersButVm paymentOrdersBut) {
        InitializeComponent();
        DataGrid.DataContext = dataGridVm;
        
        AddButton.DataContext = addButVm;
        EditButton.DataContext = editButVm;
        OpenOrderPositionsButton.DataContext = positionsBut;
        OpenPaymentOrdersButton.DataContext = paymentOrdersBut;
        DeleteButton.DataContext = deleteSoButVm;
        
        _editSoButVm = editButVm;
        _deleteSoButVm = deleteSoButVm;
        _positionsBut = positionsBut;
        _paymentOrdersBut = paymentOrdersBut;
        EventBus<IGlSubscriber>.Subscribe(this);
    }



    public Func<Task> OnReload { get; set; }

    private void SupplierOrderTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Delete:
                if (_deleteSoButVm.CanClick())
                    _deleteSoButVm.OnClickAsync();
                break;
            
            
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
        _positionsBut.NotifyCanExecute();
        _paymentOrdersBut.NotifyCanExecute();
        _deleteSoButVm.NotifyCanExecute();
    }
    
    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<SupplierOrderTableWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }
    
}
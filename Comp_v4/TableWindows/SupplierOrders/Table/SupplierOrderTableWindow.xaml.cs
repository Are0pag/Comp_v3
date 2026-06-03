using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Comp_v4._Installers;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Templates.Common;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Windows;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.SupplierOrders.Table;

public partial class SupplierOrderTableWindow : TableWindowBase, IDisposable, IReloadable, IRuntimeParamsResolver<SupplierOrderTableWindow>
{
    protected readonly AddSoButVm _addSoButVm;
    protected readonly EditSoButVm _editSoButVm;
    protected readonly DeleteSoButVm _deleteSoButVm;
    protected readonly OpenPaymentOrdersButVm _paymentOrdersBut;
    protected readonly OpenOrderPositionsButVm _positionsBut;

    public SupplierOrderTableWindow(SoDataGridVm dataGridVm, 
                                    AddSoButVm addButVm, EditSoButVm editButVm, DeleteSoButVm deleteSoButVm, 
                                    OpenOrderPositionsButVm positionsBut, OpenPaymentOrdersButVm paymentOrdersBut,
                                    FiltersVmBase filtersVm) {
        InitializeComponent();
        DataGrid.DataContext = dataGridVm;
        
        AddButton.DataContext = addButVm;
        EditButton.DataContext = editButVm;
        DeleteButton.DataContext = deleteSoButVm;
        
        OpenOrderPositionsButton.DataContext = positionsBut;
        OpenPaymentOrdersButton.DataContext = paymentOrdersBut;

        FilterTextBox.DataContext = filtersVm;
        IgnoreCaseCheckBox.DataContext = filtersVm;
        
        InfoDataGridContextMenuAddNewItemCommand.DataContext = addButVm;
        InfoDataGridContextMenuEditItemCommand.DataContext = editButVm;
        InfoDataGridContextMenuDeleteItemCommand.DataContext = deleteSoButVm;
        
        InfoDataGridContext_OpenOrderPositions_Command.DataContext = positionsBut;
        InfoDataGridContext_OpenPaymentOrders_Command.DataContext = paymentOrdersBut;

        _addSoButVm = addButVm;
        _editSoButVm = editButVm;
        _deleteSoButVm = deleteSoButVm;
        _positionsBut = positionsBut;
        _paymentOrdersBut = paymentOrdersBut;
        EventBus<IGlSubscriber>.Subscribe(this);
    }



    public Func<Task> OnReload { get; set; }

    private void SupplierOrderTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Insert:
                if (_addSoButVm.CanClick())
                    _addSoButVm.OnClickAsync();
                break;
            
            case Key.Delete:
                if (_deleteSoButVm.CanClick())
                    _deleteSoButVm.OnClickAsync();
                break;
            
            case Key.F1:
                var cb = VisualHelper.FindVisualChild<CheckBox>((Window)sender);
                cb.IsChecked = !cb.IsChecked;
                break;
            
            case Key.Escape:
                try {
                    ((Window)sender).Close();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                break;
            
        #if DEBUG
            // case Key.K:
            //     OnReload?.Invoke();
            //     break;
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
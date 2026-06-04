using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Utils.EventBus;
using Utils.WPF.Windows;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.OrderPositions.Table;

public partial class OrderPositionsTableWindow : TableWindowBase, IRuntimeParamsResolver<OrderPositionsTableWindow>
{
    private readonly EditOrderPosFormButVm _editOrderPosFormButVm;

    public OrderPositionsTableWindow(OpDataGridVm opDataGridVm, CreateOrderPosFormButVm createOrderPosFormButVm, EditOrderPosFormButVm editOrderPosFormButVm) {
        InitializeComponent();
        _editOrderPosFormButVm = editOrderPosFormButVm;
        
        DataGrid.DataContext = opDataGridVm;
        AddButton.DataContext = createOrderPosFormButVm;
        EditButton.DataContext = editOrderPosFormButVm;
        EventBus<IGlSubscriber>.Subscribe(this);

        var filtersVm = new FiltersVmBase();
        opDataGridVm.FiltersVm = filtersVm;
        FilterTextBox.DataContext = filtersVm;
        IgnoreCaseCheckBox.DataContext = filtersVm;

        Loaded += (_, _) => {
            _ = opDataGridVm.InitFilteringCollection();
        };
    }

    private void SupplierOrderTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        
    }

    private void SupplierOrderTableWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        _editOrderPosFormButVm.NotifyCanExecute();
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        
    }
    
    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPositionsTableWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }
    
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Comp_v4._Installers;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Windows;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.OrderPositions.Table;

public partial class OrderPositionsTableWindow : TableWindowBase, IRuntimeParamsResolver<OrderPositionsTableWindow>
{
    private readonly CreateOrderPosFormButVm _createOrderPosFormButVm;
    private readonly EditOrderPosFormButVm _editOrderPosFormButVm;
    private readonly DeleteOrderPositionButVm _deleteOrderPositionButVm;

    public OrderPositionsTableWindow(OpDataGridVm opDataGridVm, 
                                     
                                     CreateOrderPosFormButVm createOrderPosFormButVm, 
                                     EditOrderPosFormButVm editOrderPosFormButVm,
                                     DeleteOrderPositionButVm delBut) {
        InitializeComponent();
        _createOrderPosFormButVm = createOrderPosFormButVm;
        _editOrderPosFormButVm = editOrderPosFormButVm;
        _deleteOrderPositionButVm = delBut;
        
        DataGrid.DataContext = opDataGridVm;
        ButsAndContexMenu(createOrderPosFormButVm, editOrderPosFormButVm, delBut);

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
        switch (e.Key) {
            case Key.Insert:
                if (_createOrderPosFormButVm.CanClick())
                    _createOrderPosFormButVm.OnClickAsync();
                break;

            case Key.Delete:
                if (_deleteOrderPositionButVm.CanClick())
                    _deleteOrderPositionButVm.OnClickAsync();
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
        }
    }

    private void SupplierOrderTableWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {

    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        // Получаем элемент, по которому кликнули
        var depObj = e.OriginalSource as DependencyObject;
        if (depObj == null)
            return;

        // Ищем строку или ячейку DataGrid в дереве элементов
        var cell = VisualTreeHelper.GetParent(depObj);
        while (cell != null && !(cell is DataGridCell)) {
            cell = VisualTreeHelper.GetParent(cell);
        }

        // Если клик был по ячейке, проверяем её колонку
        if (cell is DataGridCell gridCell) {
            // Проверяем, является ли колонка ReadOnly
            if (gridCell.Column != null && gridCell.Column.IsReadOnly) {
                if (_editOrderPosFormButVm.CanClick()) {
                    _editOrderPosFormButVm.OnClickAsync();
                }
            }
        }
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<OrderPositionsTableWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }

    private void ButsAndContexMenu(CreateOrderPosFormButVm createOrderPosFormButVm, EditOrderPosFormButVm editOrderPosFormButVm, DeleteOrderPositionButVm delBut) {
        AddButton.DataContext = createOrderPosFormButVm;
        EditButton.DataContext = editOrderPosFormButVm;
        DeleteButton.DataContext = delBut;
        
        InfoDataGridContextMenuAddNewItemCommand.DataContext = createOrderPosFormButVm;
        InfoDataGridContextMenuEditItemCommand.DataContext = editOrderPosFormButVm;
        InfoDataGridContextMenuDeleteItemCommand.DataContext = delBut;
    }

    private void DataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
        _editOrderPosFormButVm.NotifyCanExecute();
        _deleteOrderPositionButVm.NotifyCanExecute();
    }
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration.Provider;
using System.Windows.Controls;
using Comp_v4.TableWindows.OrderPositions.Events;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.OrderPositions.Table.Vm;

public class OpDataGridVm : FilterGridVm<OrderPosition>, IOpTableReloadHandler, ISoPropertyChangeHandler
{
    protected SupplierOrder? _correspondingSo;

    public OpDataGridVm(IRepository<OrderPosition> repository, IFilter<OrderPosition, FiltersVmBase> filter) : base(repository, filter) {
        EventBus<IOrderPositionSubscriber>.Subscribe(this);
        EventBus<ISupplierOrdersSubscriber>.Subscribe(this);
    }

    public SoDataGridVm? SoDataGridVm { get; set; }
    
    
    private void Op_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (sender is not OrderPosition orderPosition) 
            return;
        
        switch (e.PropertyName) {
            case nameof(OrderPosition.OrderQuantity):
            case nameof(OrderPosition.UnitPrice):
                orderPosition.TotalCost = orderPosition.OrderQuantity * orderPosition.UnitPrice;
                _repository.UpdateAsync(orderPosition);
                break;
        }
    }

    public void OnOpTableReload(object? args = null) {
        _ = InitFilteringCollection();
    }

    public override async Task InitFilteringCollection() {
        if (SoDataGridVm is null)
            throw new NullReferenceException("Доигрался со scope-ами, мудила: SoDataGridVm is null");

        if (SoDataGridVm.LastSelectedSupplierOrder is null)
            throw new ProviderException();
        
        _correspondingSo = SoDataGridVm.LastSelectedSupplierOrder;
        var data = await _repository.GetAllBySupplierOrderAsync(_correspondingSo.Id);
        Items = new ObservableCollection<OrderPosition>(data);
        ItemsSorted = new ObservableCollection<OrderPosition>(Items);
        
        foreach (var op in Items)
            op.PropertyChanged += Op_PropertyChanged;
        
        OnPropertyChanged(nameof(ItemsSorted));
    }

    public void Dispose() {
        EventBus<IOrderPositionSubscriber>.Unsubscribe(this);
        EventBus<ISupplierOrdersSubscriber>.Unsubscribe(this);
    }

    public void OnOrderPositionChanged(object parameter = null) {
        if (_correspondingSo is not {} so)
            throw new ProviderException();

        so.OrderedUnitsAmount = Items.Sum(op => op.OrderQuantity);
        so.ReceivedUnitsAmount = Items.Sum(op => op.ReceivedQuantity);
        
        so.TotalOrderCost = Items.Sum(op => op.TotalCost);
        //so.TotalPayment = Items.Sum(op => op.);
        //so.TotalVatAmount = Items.Sum(op => op.);

        //so.PercentageOfTotalPayment = Items.Sum(op => op.);
        //so.PaymentStatusEnumValue = Items.Sum(op => op.);
    }

    /// <summary>
    /// не исп-ся
    /// </summary>
    public void DataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e) {
        if (e.Row.Item is not OrderPosition op) {
            Console.Error.WriteLine("DataGrid_OnBeginningEdit Op");
            throw new ProviderException();
        }
        EventBus<IOrderPositionSubscriber>.RaiseEvent<IStartEditingOpHandler>((h) => {
            h!.OnStartEditing(op);
        });
    }
}
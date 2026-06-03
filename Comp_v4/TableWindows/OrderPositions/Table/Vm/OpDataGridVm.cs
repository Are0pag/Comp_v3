using System.Collections.ObjectModel;
using System.Configuration.Provider;
using Comp_v4.TableWindows.OrderPositions.Events;
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
    


    protected override async Task LoadDataAsync() {
        await Task.Delay(100);
        
        if (SoDataGridVm is null)
            throw new NullReferenceException("Доигрался со scope-ами, мудила: SoDataGridVm is null");

        if (SoDataGridVm.LastSelectedSupplierOrder is null)
            throw new ProviderException();
        
        _correspondingSo = SoDataGridVm.LastSelectedSupplierOrder;
        var data = await _repository.GetAllBySupplierOrderAsync(_correspondingSo.Id);
        Items = new ObservableCollection<OrderPosition>(data);
        OnPropertyChanged(nameof(Items));
    }

    public void OnOpTableReload(object? args = null) {
        LoadDataAsync();
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
}
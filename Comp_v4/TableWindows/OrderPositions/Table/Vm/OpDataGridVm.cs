using System.Collections.ObjectModel;
using System.Configuration.Provider;
using Comp_v4.TableWindows.OrderPositions.Events;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.TableWindows.OrderPositions.Table.Vm;

public class OpDataGridVm : VmEnumerableInteractiveData<OrderPosition>, IOpTableReloadHandler
{
    protected readonly IRepository<OrderPosition> _repository;

    public SoDataGridVm? SoDataGridVm { get; set; }
    
    public OpDataGridVm(IRepository<OrderPosition> repository) {
        _repository = repository;
        EventBus<IOrderPositionSubscriber>.Subscribe(this);
    }

    protected override async Task LoadDataAsync() {
        await Task.Delay(100);
        
        if (SoDataGridVm is null)
            throw new NullReferenceException("Доигрался со scope-ами, мудила: SoDataGridVm is null");

        if (SoDataGridVm.LastSelectedSupplierOrder is null)
            throw new ProviderException();
        
        var data = await _repository.GetAllBySupplierOrderAsync(SoDataGridVm.LastSelectedSupplierOrder.Id);
        Items = new ObservableCollection<OrderPosition>(data);
        OnPropertyChanged(nameof(Items));
    }

    public void OnOpTableReload(object? args = null) {
        LoadDataAsync();
    }

    public void Dispose() {
        EventBus<IOrderPositionSubscriber>.Unsubscribe(this);
    }
}
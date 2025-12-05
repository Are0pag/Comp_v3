using System.Collections.ObjectModel;
using Comp_v4.TableWindows.OrderPositions.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.TableWindows.OrderPositions.Table.Vm;

public class OpDataGridVm : VmEnumerableInteractiveData<OrderPosition>, IOpTableReloadHandler
{
    protected readonly IRepository<OrderPosition> _repository;

    public OpDataGridVm(IRepository<OrderPosition> repository) {
        _repository = repository;
        EventBus<IOrderPositionSubscriber>.Subscribe(this);
    }

    protected override async Task LoadDataAsync() {
        var data = await _repository.GetAllAsync();
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
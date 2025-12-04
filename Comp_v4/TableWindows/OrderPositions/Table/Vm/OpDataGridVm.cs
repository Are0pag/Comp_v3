using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.TableWindows.OrderPositions.Table.Vm;

public class OpDataGridVm : VmEnumerableInteractiveData<OrderPosition>
{
    protected readonly IRepository<OrderPosition> _repository;

    public OpDataGridVm(IRepository<OrderPosition> repository) {
        _repository = repository;
    }

    protected override async Task LoadDataAsync() {
        var data = await _repository.GetAllAsync();
        Items = new ObservableCollection<OrderPosition>(data);
        OnPropertyChanged(nameof(Items));
    }
}
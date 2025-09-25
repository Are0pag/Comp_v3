using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.NomDict.Vm;

public class DataGridVm : VmEnumerableInteractiveData<Component>
{
    protected readonly IRepository<Component> _repository;

    public DataGridVm(IRepository<Component> repository) {
        _repository = repository;
        _ = LoadDataSync();
    }

    protected async Task LoadDataSync() {
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<Component>(items);
        OnPropertyChanged(nameof(Items));
    }
}
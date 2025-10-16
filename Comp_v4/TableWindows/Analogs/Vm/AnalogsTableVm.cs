using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.TableWindows.Analogs;

public class AnalogsTableVm : VmEnumerableInteractiveData<Analog>
{
    protected readonly IRepository<Analog> _repository;

    public AnalogsTableVm(IRepository<Analog> repository) {
        _repository = repository;
        _ = LoadDataAsync();
    }

    protected override async Task LoadDataAsync() {
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<Analog>(items);
        OnPropertyChanged(nameof(Items));
    }
}
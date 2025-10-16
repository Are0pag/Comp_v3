using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.TableWindows.Analogs;

public class AnalogsTableVm : VmEnumerableInteractiveData<Analog>
{
    protected readonly IRepository<Analog> _repository;
    protected readonly Component _component;

    public AnalogsTableVm(IRepository<Analog> repository, Component component) {
        _repository = repository;
        _component = component;
        _ = LoadDataAsync();
    }

    protected override async Task LoadDataAsync() {
        var items = await _repository.GetAnalogsFor(_component.Id);
        Items = new ObservableCollection<Analog>(items);
        OnPropertyChanged(nameof(Items));
    }
}
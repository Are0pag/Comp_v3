using System.Collections.ObjectModel;
using Comp_v4.NomDict.Events;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.NomDict.Vm;

public class DataGridVm : VmEnumerableInteractiveData<Component>, IUiRefreshHandler
{
    protected readonly IRepository<Component> _repository;

    public DataGridVm(IRepository<Component> repository) {
        _repository = repository;
        _ = LoadDataAsync();
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }

    protected override async Task LoadDataAsync() {
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<Component>(items);
        OnPropertyChanged(nameof(Items));
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }

    public void RefreshUi(object? args) {
        if (args is not Component component) 
            return;
        Items.Add(component);
        OnPropertyChanged(nameof(Items));
    }
}
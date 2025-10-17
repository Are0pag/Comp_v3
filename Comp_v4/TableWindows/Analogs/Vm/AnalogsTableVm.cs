using System.Collections.ObjectModel;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.TableWindows.Analogs;

public class AnalogsTableVm : VmEnumerableInteractiveData<Analog>, ISaveHandler
{
    protected readonly IRepository<Analog> _repository;
    protected readonly Component _component;

    public AnalogsTableVm(IRepository<Analog> repository, Component component) {
        _repository = repository;
        _component = component;
        _ = LoadDataAsync();
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    protected override async Task LoadDataAsync() {
        var items = await _repository.GetAnalogsFor(_component.Id);
        Items = new ObservableCollection<Analog>(items);
        OnPropertyChanged(nameof(Items));
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public Task Save(TaskCompletionSource tcs, Analog analog) {
        Items.Add(analog);
        tcs.SetResult();
        return Task.CompletedTask;
    }
}
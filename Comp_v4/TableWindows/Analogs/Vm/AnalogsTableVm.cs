using System.Collections.ObjectModel;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v4.TableWindows.Analogs;

public class AnalogsTableVm : VmEnumerableInteractiveData<Analog>, IAnalogSaveHandler, IRuntimeParamsContainer<Component>, IAnalogTableLoadHandler
{
    protected readonly IRepository<Analog> _repository;
    protected Component _component;

    public AnalogsTableVm(IRepository<Analog> repository) {
        _repository = repository;
        _ = LoadDataAsync();
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    protected override async Task LoadDataAsync() {
        var items = await _repository.GetAnalogsFor(RuntimeParam.Id);
        Items = new ObservableCollection<Analog>(items);
        OnPropertyChanged(nameof(Items));
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public async Task OnLoad(TaskCompletionSource tcs) {
        Items.Clear();
        await LoadDataAsync();
        tcs.SetResult();
    }

    public Task Save(TaskCompletionSource tcs, Analog analog) {
        if (Items.FirstOrDefault(a => a.Id == analog.Id) is not { } newItem)
          Items.Add(analog);
        
        tcs.SetResult();
        return Task.CompletedTask;
    }
    
    public Component RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<Component>>(r => { r.ResolveRuntimeParams(this); });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _component;
        }
        set {
            _component = value;
        }
    }
}
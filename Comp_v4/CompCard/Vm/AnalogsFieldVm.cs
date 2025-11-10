using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v4._Installers;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.EventBus;

namespace Comp_v4.CompCard.Vm;

public class AnalogsFieldVm : ObservableObject, TableWindows.Analogs.Events.ISaveHandler, IRuntimeParamsContainer<Component>
{
    protected readonly IRepository<Analog> _analogsRepository;
    protected Component _component;
    protected int _analogsCount;
    protected string _label = "Аналоги:";

    public AnalogsFieldVm(IRepository<Analog> analogsRepository) {
        _analogsRepository = analogsRepository;
        _ = GetAnalogsCount();
        EventBus<TableWindows.Analogs.Events.IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public async Task GetAnalogsCount() {
        AnalogsCount = await _analogsRepository.GetAnalogsCount(RuntimeParam.Id);
    }

    public int AnalogsCount {
        get => _analogsCount;
        protected set {
            if (_analogsCount == value) return;
            _analogsCount = value;
            OnPropertyChanged();
        }
    }

    public string Label {
        get => _label;
        set {
            if (_label == value) return;
            _label = value;
            OnPropertyChanged();
        }
    }

    public void Dispose() {
        EventBus<TableWindows.Analogs.Events.IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public Task Save(TaskCompletionSource tcs, Analog analog) {
        if (analog.SourceComponent.Id == RuntimeParam.Id) 
            AnalogsCount += 1;
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
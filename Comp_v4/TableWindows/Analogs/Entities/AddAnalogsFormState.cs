using Comp_v4._Installers;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.View;
using Comp.Db.Contracts;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class AddAnalogsFormState : BaseAnalogsFormState, IGetResultOfSelectionHanlder, IRuntimeParamsContainer<Analog>
{
    protected readonly IRepository<Analog> _analogRepository;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    protected readonly IServiceProvider _serviceProvider;
    protected Analog _analog;
    protected TaskCompletionSource _butTcs;

    public AddAnalogsFormState(IWindowOrderLocator windowOrderLocator, IRepository<Analog> analogRepository, IServiceProvider serviceProvider) {
        _windowOrderLocator = windowOrderLocator;
        _analogRepository = analogRepository;
        _serviceProvider = serviceProvider;
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }

    public override Task OnStartSelectingAnalog(object? parameter = null) {
        if (parameter is not TaskCompletionSource butTcs)
            throw new ArgumentException("parameter must be a TaskCompletionSource");
        _butTcs = butTcs;
        
        var completionSource = new TaskCompletionSource<Component>();
        EventBus<INomDictWindowSubscriber>
           .RaiseEvent<IGridSelectingStateHandler>(h => {
                h?.OnSelecting(completionSource);
            });
        return Task.CompletedTask;
    }
    
    

    public override async Task Save(AnalogsForm form) {
        try {
            await _analogRepository.AddAsync(_analog);
        }
        catch (Exception ex) {
            throw;
        }
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }

    public void OnGetResultOfSelection(Component component) {
        RuntimeParam.RelatedComponent = component;
        _windowOrderLocator.MoveToBack<NomDictWindow>();
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
        _butTcs.SetResult();
    }
    
    public Analog RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<Analog>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _analog;
        }
        set {
            _analog = value;
        }
    }
}
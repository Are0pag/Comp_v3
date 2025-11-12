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

public class AddAnalogsFormState : BaseAnalogsFormState
{
    protected readonly IRepository<Analog> _analogRepository;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    protected readonly IServiceProvider _serviceProvider;
    protected Analog _analog;

    public AddAnalogsFormState(IWindowOrderLocator windowOrderLocator, IRepository<Analog> analogRepository, IServiceProvider serviceProvider) {
        _windowOrderLocator = windowOrderLocator;
        _analogRepository = analogRepository;
        _serviceProvider = serviceProvider;
    }

    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        if (parameter is not TaskCompletionSource butTcs)
            throw new ArgumentException("parameter must be a TaskCompletionSource");
        
        var completionSource = new TaskCompletionSource<Component>();
        EventBus<INomDictWindowSubscriber>
           .RaiseEvent<IGridSelectingStateHandler>(h => {
                h?.OnSelecting(completionSource);
            });
        _analog.RelatedComponent = await completionSource.Task;
        _windowOrderLocator.MoveToBack<NomDictWindow>();
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
        butTcs.SetResult();
    }

    public override async Task Save(AnalogsForm form) {
        try {
            await _analogRepository.AddAsync(_analog);
        }
        catch (Exception ex) {
            throw;
        }
    }
}
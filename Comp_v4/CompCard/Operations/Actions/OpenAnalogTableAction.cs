using Comp_v4.CompCard.Vm.Buttons;
using Comp_v4.TableWindows.Analogs;
using Comp_v4.TableWindows.Analogs.Actions;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using DI;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.CompCard.Operations.Actions;

public class OpenAnalogTableAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    public OpenAnalogTableAction(AnalogsFieldButtonVm buttonVm, IServiceProvider serviceProvider, IWindowOrderLocator windowOrderLocator) : base(buttonVm) {
        _serviceProvider = serviceProvider;
        _windowOrderLocator = windowOrderLocator;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var window = _serviceProvider.GetRequiredService<AnalogsTableWindow>();
        var parentWindow = new InstanceContainer<CompCardWindow>().RuntimeParam;
        window.Owner = parentWindow;
        _windowOrderLocator.RegisterWindow(window);
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
            _windowOrderLocator.UnregisterWindow(window);
        };
        
        _serviceProvider.GetRequiredService<AddAnalogAction>();
        _serviceProvider.GetRequiredService<EditAnalogAction>();

        await ReLoad();
        
        WindowService.BindChildToParent(parentWindow, window);
        window.Show();
        
        await tcs.Task;
    }
    
    private static async Task ReLoad() {
        var loadingTcs = new TaskCompletionSource();
        EventBus<IAnalogsTableWindowSubscriber>
           .RaiseEvent<IAnalogTableLoadHandler>(h => {
                h?.OnLoad(loadingTcs);
            });
        await loadingTcs.Task;
    }

    public override bool CanPerform() {
        return true;
    }
}
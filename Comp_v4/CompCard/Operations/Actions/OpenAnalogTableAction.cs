using Comp_v4.CompCard.Vm.Buttons;
using Comp_v4.TableWindows.Analogs;
using DI;
using Microsoft.Extensions.DependencyInjection;
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
        _windowOrderLocator.RegisterWindow(window);
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        
        window.Show();
        await tcs.Task;
    }

    public override bool CanPerform() {
        return true;
    }
}
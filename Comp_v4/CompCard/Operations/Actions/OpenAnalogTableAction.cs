using Comp_v4.CompCard.Vm.Buttons;
using Comp_v4.TableWindows.Analogs;
using DI;
using Utils.WPF;

namespace Comp_v4.CompCard.Operations.Actions;

public class OpenAnalogTableAction : BaseAsyncActionButtonInvoked
{
    protected readonly AreopagContainer _tableContainer;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    public OpenAnalogTableAction(AnalogsFieldButtonVm buttonVm, AreopagContainer tableContainer, IWindowOrderLocator windowOrderLocator) : base(buttonVm) {
        _tableContainer = tableContainer;
        _windowOrderLocator = windowOrderLocator;
    }

    public override async Task PerformAsync(object? parameter) {
        var window = _tableContainer.BeginScope<AnalogsTableWindow>();
        _windowOrderLocator.RegisterWindow(window);
        window.Closed += (sender, args) => {
            _tableContainer.ReleaseScope<AnalogsTableWindow>();
        };
        window.Show();
    }

    public override bool CanPerform() {
        return true;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
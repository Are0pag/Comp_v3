using Comp_v4.CompCard.Vm.Buttons;
using Comp_v4.TableWindows.Analogs;
using Utils.WPF;
using Utils.WPF.Buttons;
using WPF.Services;

namespace Comp_v4.CompCard.Operations.Actions;

public class OpenAnalogTableAction : BaseAsyncActionButtonInvoked
{
    protected readonly AreopagContainer _tableContainer;
    public OpenAnalogTableAction(AnalogsFieldButtonVm buttonVm, AreopagContainer tableContainer) : base(buttonVm) {
        _tableContainer = tableContainer;
    }

    public override async Task PerformAsync(object? parameter) {
        var window = _tableContainer.BeginScope<AnalogsTableWindow>();
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
using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class AddCounterpartyAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceProvider _serviceProvider;

    public AddCounterpartyAction(AddCounterpartyButVm button, IServiceProvider serviceProvider) : base(button) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;

        var window = _serviceProvider.GetRequiredService<CounterpartyFormWindow>();

        _serviceProvider.GetRequiredService<FormCp>();
        _serviceProvider.GetRequiredService<SaveCpFormAction>();

        window.Closed += (sender, args) => { _currentTcs.TrySetResult(); };
        window.Show();

        await _currentTcs.Task;
    }
}
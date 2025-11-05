using Comp_v4.TableWindows.Counterparties.Table;
using Comp_v4.TableWindows.Counterparties.Table.Actions;
using Comp_v4.TableWindows.Counterparties.Table.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class CounterpartySelectAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceProvider _serviceProvider;

    public CounterpartySelectAction(CounterpartySelectButVm button, IServiceProvider serviceProvider) : base(button) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;

        var window = _serviceProvider.GetRequiredService<CounterpartyTableWindow>();

        _serviceProvider.GetRequiredService<TableCounterparty>();
        _serviceProvider.GetRequiredService<AddCounterpartyAction>();
        _serviceProvider.GetRequiredService<EditCounterpartyAction>();

        window.Closed += (sender, args) => {
            _currentTcs.TrySetResult();
        };
        window.Show();

        await _currentTcs.Task;
    }
}
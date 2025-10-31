using Comp_v4.TableWindows.Counterparties.Table;
using Comp_v4.TableWindows.Counterparties.Table.Actions;
using Comp_v4.TableWindows.Counterparties.Table.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class CounterpartySelectAction : BaseActionAsyncScopeHandler
{
    public CounterpartySelectAction(CounterpartySelectButVm button, IServiceScopeFactory scopeFactory) : base(button, scopeFactory) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<CounterpartyTableWindow>();
            
            scope.ServiceProvider.GetRequiredService<TableCounterparty>();
            scope.ServiceProvider.GetRequiredService<AddCounterpartyAction>().ParentScope = scope;
            scope.ServiceProvider.GetRequiredService<EditCounterpartyAction>().ParentScope = scope;

            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }
}
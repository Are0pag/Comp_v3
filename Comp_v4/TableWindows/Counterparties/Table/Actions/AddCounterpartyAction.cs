using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class AddCounterpartyAction : BaseActionAsyncScopeHandler
{
    public AddCounterpartyAction(AddCounterpartyButVm button, IServiceScopeFactory scopeFactory) : base(button, scopeFactory) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<CounterpartyFormWindow>();

            scope.ServiceProvider.GetRequiredService<FormCp>();
            scope.ServiceProvider.GetRequiredService<SaveCpFormAction>();

            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }
}
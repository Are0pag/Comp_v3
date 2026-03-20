using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class AddCounterpartyAction : BaseActionAsyncScopeHandler
{
    protected readonly CounterpartyTableWindow _counterpartyTableWindow;
    public AddCounterpartyAction(AddCounterpartyButVm button, IServiceScopeFactory scopeFactory, CounterpartyTableWindow counterpartyTableWindow) 
        : base(button, scopeFactory) {
        _counterpartyTableWindow = counterpartyTableWindow;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<CounterpartyFormWindow>();

            scope.ServiceProvider.GetRequiredService<FormCp>();
            scope.ServiceProvider.GetRequiredService<SaveCpFormAction>();

            window.Closed += async (sender, args) => {
                _currentTcs.TrySetResult();
                await Task.Delay(AppConfig.TCS_EXECUTION_DELAY);
                _counterpartyTableWindow.OnReload?.Invoke();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }
}
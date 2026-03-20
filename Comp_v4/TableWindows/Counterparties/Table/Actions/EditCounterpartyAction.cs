using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class EditCounterpartyAction : BaseActionAsyncScopeHandler
{
    protected readonly CounterpartyDataGridVm _dataGridVm;
    protected readonly CounterpartyTableWindow _counterpartyTableWindow;
    public EditCounterpartyAction(EditCounterpartyButVm button, IServiceScopeFactory scopeFactory, CounterpartyDataGridVm dataGridVm, CounterpartyTableWindow counterpartyTableWindow) : base(button, scopeFactory) {
        _dataGridVm = dataGridVm;
        _counterpartyTableWindow = counterpartyTableWindow;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<CounterpartyFormWindow>();
            
            var targetItem = scope.ServiceProvider.GetRequiredService<Counterparty>();
            targetItem.PopulateFrom(_dataGridVm.SelectedItem!);

            var form = scope.ServiceProvider.GetRequiredService<FormCp>();
            await form.ChangeState(scope.ServiceProvider.GetRequiredService<EditCpFormState>(), form);
            
            scope.ServiceProvider.GetRequiredService<SaveCpFormAction>();

            var reloadTcs = new TaskCompletionSource(); // Новый TCS для координации

            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
                reloadTcs.SetResult(); // Сигнализируем, что закрытие обработано
            };

            window.Show();
    
            await _currentTcs.Task; // Ждем завершения формы
            if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("awaiting form task completed");

            await reloadTcs.Task;                            // Ждем пока обработчик Closed выполнится

            if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("Table started reloading");
            _counterpartyTableWindow.OnReload?.Invoke();
        }
    }

    public override bool CanPerform() {
        return base.CanPerform() && _dataGridVm.SelectedItem != null;
    }
}
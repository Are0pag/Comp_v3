using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class EditCounterpartyAction : BaseActionAsyncScopeHandler
{
    protected readonly CounterpartyDataGridVm _dataGridVm;
    public EditCounterpartyAction(BaseButtonAdvanced button, IServiceScopeFactory scopeFactory, CounterpartyDataGridVm dataGridVm) : base(button, scopeFactory) {
        _dataGridVm = dataGridVm;
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

            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }

    public override bool CanPerform() {
        return base.CanPerform() && _dataGridVm.SelectedItem != null;
    }
}
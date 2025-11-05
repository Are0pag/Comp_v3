using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class EditCounterpartyAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly CounterpartyDataGridVm _dataGridVm;

    public EditCounterpartyAction(EditCounterpartyButVm button, IServiceProvider serviceProvider, CounterpartyDataGridVm dataGridVm) : base(button) {
        _serviceProvider = serviceProvider;
        _dataGridVm = dataGridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;

        var window = _serviceProvider.GetRequiredService<CounterpartyFormWindow>();

        var targetItem = _serviceProvider.GetRequiredService<Counterparty>();
        targetItem.PopulateFrom(_dataGridVm.SelectedItem!);

        var form = _serviceProvider.GetRequiredService<FormCp>();
        await form.ChangeState(_serviceProvider.GetRequiredService<EditCpFormState>(), form);

        _serviceProvider.GetRequiredService<SaveCpFormAction>();

        window.Closed += (sender, args) => { _currentTcs.TrySetResult(); };
        window.Show();

        await _currentTcs.Task;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _dataGridVm.SelectedItem != null;
    }
}
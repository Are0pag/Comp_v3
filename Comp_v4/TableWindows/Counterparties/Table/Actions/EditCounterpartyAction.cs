using Comp_v4.Entry;
using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class EditCounterpartyAction : BaseActionAsyncSelfWaiting
{
    protected readonly CounterpartyDataGridVm _dataGridVm;
    protected readonly IServiceProvider _serviceProvider;
    protected TaskCompletionSource _tcs;
    public EditCounterpartyAction(EditCounterpartyButVm button, CounterpartyDataGridVm dataGridVm, IServiceProvider serviceProvider) : base(button) {
        _dataGridVm = dataGridVm;
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _tcs = tcs;
        var counterparty = _dataGridVm.SelectedItem!;
        var window = ActivatorUtilities.CreateInstance<CounterpartyFormWindow>(_serviceProvider, counterparty);
        WindowServiceHelper.Register<EntryWindow, CounterpartyTableWindow>(window);

        _serviceProvider.GetRequiredService<SaveCpFormAction>().CurrentCounterparty = counterparty;
        _serviceProvider.GetRequiredService<CancelEditCpFormAction>().CurrentCounterparty = counterparty;
        
        var form = _serviceProvider.GetRequiredService<FormCp>();
        await form.ChangeState(_serviceProvider.GetRequiredService<EditCpFormState>(), form);

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
        await _tcs.Task;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _dataGridVm.SelectedItem != null;
    }
}
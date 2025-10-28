using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class EditCounterpartyAction : BaseActionAsyncSelfWaiting
{
    protected readonly CounterpartyDataGridVm _dataGridVm;
    public EditCounterpartyAction(EditCounterpartyButVm button, CounterpartyDataGridVm dataGridVm) : base(button) {
        _dataGridVm = dataGridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        var tasks = new List<Task>();

        EventBus<ICounterpartySubscriber>.RaiseEvent<ICounterpartyFormHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                if (_dataGridVm.SelectedItem is not { } counterparty)
                    throw new Exception();
                h?.Open<EditCpFormState>(tcs, counterparty);
            }
            catch (Exception ex) {
                subscriberTcs.TrySetException(ex);
            }
        });

        await Task.WhenAll(tasks);
        _currentTcs.TrySetResult();
    }

    public override bool CanPerform() {
        return base.CanPerform() && _dataGridVm.SelectedItem != null;
    }
}
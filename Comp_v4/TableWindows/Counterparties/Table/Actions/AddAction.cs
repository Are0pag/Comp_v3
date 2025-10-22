using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class AddAction : BaseActionAsyncCompletion<Counterparty>
{
    public AddAction(AddCounterpartyButVm button) : base(button) {
    }

    public override async void Perform(TaskCompletionSource<Counterparty> tcs) {
        var tasks = new List<Task>();

        EventBus<ICounterpartySubscriber>.RaiseEvent<ICounterpartyFormHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.Open<CreateFormState>(subscriberTcs, new Counterparty());
            }
            catch (Exception ex) {
                subscriberTcs.TrySetException(ex);
            }
        });

        await Task.WhenAll(tasks);
    }

    public override bool CanPerform() {
        return true;
    }
}
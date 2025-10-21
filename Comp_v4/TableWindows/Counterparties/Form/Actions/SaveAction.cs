using Comp_v4.TableWindows.Counterparties.Form.Events;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Actions;

public class SaveAction : BaseActionAsyncCompletion<Counterparty>
{
    public SaveAction(SaveButVm button) : base(button) {
    }

    public override async void Perform(TaskCompletionSource<Counterparty> tcs) {
        var tasks = new List<Task>();

        EventBus<ICounterpartySubscriber>.RaiseEvent<ISaveHandler>(h => {
            var subscriberTcs = new TaskCompletionSource<Counterparty>();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.Save(subscriberTcs);
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
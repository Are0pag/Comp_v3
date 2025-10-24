using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Actions;

public class SaveAction : BaseActionAsyncCompletion<Counterparty>
{
    protected readonly Counterparty _counterparty;
    public SaveAction(SaveButVm button, Counterparty counterparty) : base(button) {
        _counterparty = counterparty;
    }

    public override async void Perform(TaskCompletionSource<Counterparty> tcs) {
        var tasks = new List<Task>();

        EventBus<ICounterpartySubscriber>.RaiseEvent<ISaveHandler>(h => {
            var subscriberTcs = new TaskCompletionSource<Counterparty>();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.Save(subscriberTcs, _counterparty);
            }
            catch (Exception ex) {
                subscriberTcs.TrySetException(ex);
            }
        });

        await Task.WhenAll(tasks);
        tcs.TrySetResult(_counterparty);
    }

    public override bool CanPerform() {
        return true;
    }
}
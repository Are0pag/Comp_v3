using Comp_v4.Entry.Events;
using Comp_v4.Entry.Vm.Buts;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Vm.Actions;

public class OpenNomDictAction : BaseActionAsyncCompletion
{
    public OpenNomDictAction(NomDictButVm button) : base(button) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var tasks = new List<Task>();

        EventBus<IEntrySubscriber>.RaiseEvent<IOpenNomDictHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.OpenNomDict(subscriberTcs);
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
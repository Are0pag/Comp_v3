using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class SaveAction : BaseActionAsyncCompletion
{
    public SaveAction(SaveButVm button) : base(button) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var tasks = new List<Task>();

        EventBus<ISupplierOrdersSubscriber>.RaiseEvent<ICreateSupplierOrdersHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.OnCreateSupplierOrder(subscriberTcs);
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
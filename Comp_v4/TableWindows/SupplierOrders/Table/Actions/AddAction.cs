using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class AddAction : BaseActionAsyncCompletion
{
    public AddAction(AddButVm button) : base(button) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var tasks = new List<Task>();

        EventBus<ISupplierOrdersSubscriber>.RaiseEvent<IFormHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.OpenForm<CreateFormState>(subscriberTcs);
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
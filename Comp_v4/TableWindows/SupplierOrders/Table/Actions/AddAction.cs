using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class AddAction : BaseActionAsyncCompletion
{
    protected readonly IFormHandler _formHandler;
    protected TaskCompletionSource? _currentTcs;
    public AddAction(AddButVm button, IFormHandler formHandler) : base(button) {
        _formHandler = formHandler;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        //var subscriberTcs = new TaskCompletionSource();
        await _formHandler.OpenForm<CreateFormState>(_currentTcs, new SupplierOrder());
        await _currentTcs.Task;
        
        /*var tasks = new List<Task>();

        EventBus<ISupplierOrdersSubscriber>.RaiseEvent<IFormHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.OpenForm<CreateFormState>(subscriberTcs, new SupplierOrder());
            }
            catch (Exception ex) {
                subscriberTcs.TrySetException(ex);
            }
        });

        await Task.WhenAll(tasks);*/
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}
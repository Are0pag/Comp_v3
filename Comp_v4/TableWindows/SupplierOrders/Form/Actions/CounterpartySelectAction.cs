using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class CounterpartySelectAction : BaseActionAsyncCompletion
{
    public CounterpartySelectAction(CounterpartySelectButVm button) : base(button) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        throw new NotImplementedException();
    }

    public override bool CanPerform() {
        throw new NotImplementedException();
    }
}
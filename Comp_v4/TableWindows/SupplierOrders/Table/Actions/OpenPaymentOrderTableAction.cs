using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class OpenPaymentOrderTableAction : BaseActionAsyncSelfWaiting
{
    public OpenPaymentOrderTableAction(OpenPaymentOrdersButVm button) : base(button) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        throw new NotImplementedException();
    }
}
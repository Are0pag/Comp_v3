using Comp_v4.TableWindows.PaymentOrders.Table.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Actions;

public class AddPoAction : BaseActionAsyncSelfWaiting
{
    protected readonly PaymentOrderTable _table;
    public AddPoAction(AddPaymentOrderButVm button, PaymentOrderTable table) : base(button) {
        _table = table;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _table.AddItem(tcs);
    }
}
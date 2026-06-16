using Comp_v4.TableWindows.PaymentOrders.Table.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Actions;

public class EditPoAction : BaseActionAsyncSelfWaiting
{
    protected readonly PaymentOrderTable _table;
    protected readonly PaymentOrdersGridVm _gridVm;
    
    public EditPoAction(EditPaymentOrderButVm button, PaymentOrderTable table, PaymentOrdersGridVm gridVm) : base(button) {
        _table = table;
        _gridVm = gridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _table.EditItem(tcs, _gridVm.SelectedItem!);
    }

    public override bool CanPerform() {
        return base.CanPerform() && _gridVm.SelectedItem != null;
    }
}
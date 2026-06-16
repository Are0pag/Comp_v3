using Comp_v4.TableWindows.PaymentOrders.Form.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Form.Actions;

public class CancelPoAction : BaseActionAsyncSelfWaiting
{
    protected readonly PaymentOrderForm _form;
    
    public CancelPoAction(CancelPaymentOrderButVm button, PaymentOrderForm form) : base(button) {
        _form = form;
    }

    public PaymentOrder Po { get; set; }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _form.Cancel(tcs, Po);
    }
}
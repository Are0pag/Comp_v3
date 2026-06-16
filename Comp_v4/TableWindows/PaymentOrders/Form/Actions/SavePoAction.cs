using Comp_v4.TableWindows.PaymentOrders.Form.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Form.Actions;

public class SavePoAction : BaseActionAsyncSelfWaiting
{
    protected readonly PaymentOrderForm _form;
    protected readonly PaymentOrdersGridVm _gridVm;
    
    public SavePoAction(SavePaymentOrderButVm button, PaymentOrderForm form, PaymentOrdersGridVm gridVm) : base(button) {
        _form = form;
        _gridVm = gridVm;
    }

    public PaymentOrder Po { get; set; } 

    public override async Task Perform(TaskCompletionSource tcs) {
        await _form.Save(tcs, Po);
        new InstanceContainer<PaymentOrderFormWindow>().RuntimeParam.Close();
        _gridVm.AddItem(Po);
    }

    public override bool CanPerform() {
        if (Po == null) {
            throw new NullReferenceException("Po cannot be null");
        }
        return base.CanPerform();
    }
}
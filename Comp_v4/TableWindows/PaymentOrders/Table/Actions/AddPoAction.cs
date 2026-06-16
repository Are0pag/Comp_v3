using Comp_v4.TableWindows.PaymentOrders.Table.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Actions;

public class AddPoAction : BaseActionAsyncSelfWaiting
{
    protected readonly PaymentOrderTable _table;
    public AddPoAction(AddPaymentOrderButVm button, PaymentOrderTable table) : base(button) {
        _table = table;
    }
    
    public SupplierOrder? CurrentSo { get; set; }

    public override async Task Perform(TaskCompletionSource tcs) {
        if (CurrentSo == null)
            throw new NullReferenceException("CurrentSo is null");
        await _table.AddItem(tcs, new PaymentOrder() {
            Order = CurrentSo,
            OrderId = CurrentSo.Id,
            Date = DateTime.Now,
        });
    }

    public override bool CanPerform() {
        return base.CanPerform() && CurrentSo != null; 
    }

    public void Dispose() {
        
    }

}
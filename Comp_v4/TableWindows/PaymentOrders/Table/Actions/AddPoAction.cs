using Comp_v4.TableWindows.PaymentOrders.Table.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Actions;

public class AddPoAction : BaseActionAsyncSelfWaiting, ISelectedSoRequester
{
    protected readonly PaymentOrderTable _table;
    public AddPoAction(AddPaymentOrderButVm button, PaymentOrderTable table) : base(button) {
        _table = table;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        EventBus<ISupplierOrdersSubscriber>
           .RaiseEvent<ISelectedSoContainer>(c => c?.GetSelectedSoInfo(this));
        if (CurrentSo == null)
            throw new NullReferenceException("CurrentSo is null");
        await _table.AddItem(tcs, new PaymentOrder() {
            Order = CurrentSo,
            OrderId = CurrentSo.Id
        });
    }

    public override bool CanPerform() {
        EventBus<ISupplierOrdersSubscriber>
           .RaiseEvent<ISelectedSoContainer>(c => c?.GetSelectedSoInfo(this));

        return base.CanPerform() && CurrentSo != null; 
    }

    public void Dispose() {
        
    }

    public SupplierOrder? CurrentSo { get; set; }
}
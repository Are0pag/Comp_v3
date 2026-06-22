using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class CancelFormAction : BaseActionAsyncCompletion
{
    protected SupplierOrder _cash;
    protected SupplierOrder _origin;
    
    public CancelFormAction(CancelButtonFormVm button) : base(button) {
    }

    public void SetCash(SupplierOrder supplierOrder) {
        _cash = supplierOrder.CopyTo(new SupplierOrder());
        _origin = supplierOrder;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _cash.CopyTo(_origin);
        tcs.TrySetResult();
        new InstanceContainer<SupplierOrderFormWindow>().RuntimeParam.Close();
    }

    public override bool CanPerform() {
        return true;
    }
}
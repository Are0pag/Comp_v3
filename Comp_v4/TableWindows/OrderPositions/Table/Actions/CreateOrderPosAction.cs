using Comp_v4.TableWindows.OrderPositions.Table.Entities;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Table.Actions;

public class CreateOrderPosAction : BaseActionAsyncSelfWaiting
{
    protected readonly OpTable _opTable;
    
    public SoDataGridVm? SoDataGridVm { get; set; }
    
    public CreateOrderPosAction(CreateOrderPosFormButVm button, OpTable opTable) : base(button) {
        _opTable = opTable;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        if (SoDataGridVm is null)
            throw new NullReferenceException("Доигрался со scope-ами, мудила: SoDataGridVm is null");
        if (SoDataGridVm.LastSelectedSupplierOrder is not { } so)
            throw new ArgumentException();
        await _opTable.Create(tcs, so);
    }
}
using Comp_v4.TableWindows.OrderPositions.Table.Entities;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Table.Actions;

public class CreateOrderPosAction : BaseActionAsyncSelfWaiting
{
    protected readonly OpTable _opTable;
    public CreateOrderPosAction(CreateOrderPosFormButVm button, OpTable opTable) : base(button) {
        _opTable = opTable;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _opTable.Create(tcs);
    }
}
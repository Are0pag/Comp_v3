using Comp_v4.TableWindows.OrderPositions.Table.Entities;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Table.Actions;

public class EditOrderPosAction : BaseActionAsyncSelfWaiting
{
    protected readonly OpTable _opTable;
    protected readonly OpDataGridVm _opDataGridVm;
    
    public EditOrderPosAction(EditOrderPosFormButVm button, OpTable opTable, OpDataGridVm opDataGridVm) : base(button) {
        _opTable = opTable;
        _opDataGridVm = opDataGridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        if (_opDataGridVm.SelectedItem is not {} item)
            throw new ApplicationException("Selected item was not an order position");
        await _opTable.Edit(tcs, item);
    }

    public override bool CanPerform() {
        return base.CanPerform() && _opDataGridVm.SelectedItem != null;
    }
}
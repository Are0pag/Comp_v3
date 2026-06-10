using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Entities;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Actions;

public class EditAnalogAction : BaseActionAsyncSelfWaiting
{
    protected readonly AnalogsTable _analogsTable;
    protected readonly AnalogsTableVm _analogsTableVm;
    public EditAnalogAction(EditAnalogButVm button, AnalogsTable analogsTable, AnalogsTableVm analogsTableVm) : base(button) {
        _analogsTable = analogsTable;
        _analogsTableVm = analogsTableVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _analogsTable.Edit(tcs);
    }

    public override bool CanPerform() {
        return base.CanPerform() && _analogsTableVm.SelectedItem != null;
    }
}
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Entities;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Actions;

public class EditAnalogAction : BaseActionAsyncSelfWaiting
{
    protected readonly AnalogsTable _analogsTable;
    public EditAnalogAction(EditAnalogButVm button, AnalogsTable analogsTable) : base(button) {
        _analogsTable = analogsTable;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _analogsTable.Edit(tcs);
    }
}
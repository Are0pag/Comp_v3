using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Entities;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Actions;

public class AddAnalogAction : BaseActionAsyncSelfWaiting
{
    protected readonly AnalogsTable _table;
    
    public AddAnalogAction(AddAnalogButtonVm button, AnalogsTable table) : base(button) {
        _table = table;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _table.Add(tcs);
    }
}
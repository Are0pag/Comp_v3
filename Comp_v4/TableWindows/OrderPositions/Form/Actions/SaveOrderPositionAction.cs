using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Form.Actions;

public class SaveOrderPositionAction : BaseActionAsyncSelfWaiting
{
    public SaveOrderPositionAction(SaveOrderPositionButVm button) : base(button) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        throw new NotImplementedException();
    }
}
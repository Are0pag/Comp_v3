using Comp_v4.NomDict.Events;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Form.Actions;

public class SelectPositionAction : BaseActionAsyncSelfWaiting
{
    protected TaskCompletionSource? _butTcs;
    
    public SelectPositionAction(SelectPositionButVm button) : base(button) {
    }

    public override Task Perform(TaskCompletionSource tcs) {
        _butTcs = tcs;
        EventBus<INomDictWindowSubscriber>
           .RaiseEvent<IGridSelectingStateHandler>(h => {
                h?.OnSelecting(new TaskCompletionSource<Component>(), this.GetType());
            });
        return Task.CompletedTask;
    }
}
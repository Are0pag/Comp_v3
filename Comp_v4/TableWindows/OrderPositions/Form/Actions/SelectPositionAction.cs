using Comp_v4.NomDict.Events;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Form.Actions;

public class SelectPositionAction : BaseActionAsyncSelfWaiting, IGetResultOfSelectionHanlder
{
    protected TaskCompletionSource? _butTcs;
    
    public SelectPositionAction(SelectPositionButVm button) : base(button) {
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }

    public override Task Perform(TaskCompletionSource tcs) {
        _butTcs = tcs;
        EventBus<INomDictWindowSubscriber>
           .RaiseEvent<IGridSelectingStateHandler>(h => {
                h?.OnSelecting(new TaskCompletionSource<Component>(), this.GetType());
            });
        return Task.CompletedTask;
    }

    public void OnGetResultOfSelection(Component component, Type requesterType) {
        if (requesterType != GetType())
            return;
        if (_butTcs is null)
            return;
        
        
        
        _butTcs.SetResult();
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }
}
using Comp_v4._Installers;
using Comp_v4.Entry.Vm.Buts;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.View;
using Comp_v4.TableWindows.OrderPositions.Events;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Form.Actions;

public class SelectPositionAction : BaseActionAsyncSelfWaiting, IGetResultOfSelectionHanlder, IRuntimeParamsContainer<OrderPositionVm>
{
    protected readonly NomDictButVm _nomDictButVm;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    
    protected TaskCompletionSource? _butTcs;
    
    public SelectPositionAction(SelectPositionButVm button, NomDictButVm nomDictButVm, IWindowOrderLocator windowOrderLocator) : base(button) {
        _nomDictButVm = nomDictButVm;
        _windowOrderLocator = windowOrderLocator;
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }

    public override Task Perform(TaskCompletionSource tcs) {
        _nomDictButVm.OnClickAsync();
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
        
        _windowOrderLocator.MoveToBack<NomDictWindow>();
        
        RuntimeParam.Position = component;
        
        _butTcs.SetResult();
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }
    
    protected OrderPositionVm _orderPosition;
    public OrderPositionVm RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<OrderPositionVm>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _orderPosition;
        }
        set {
            _orderPosition = value;
        }
    }
}
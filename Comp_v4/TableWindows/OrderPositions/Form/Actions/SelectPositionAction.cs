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
    protected Type? _requesterType;
    
    public SelectPositionAction(SelectPositionButVm button, NomDictButVm nomDictButVm, IWindowOrderLocator windowOrderLocator) : base(button) {
        _nomDictButVm = nomDictButVm;
        _windowOrderLocator = windowOrderLocator;
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }

    public override Task Perform(TaskCompletionSource tcs) {
        _nomDictButVm.OnClickAsync();
        _butTcs = tcs;
        _requesterType = GetType();
        EventBus<INomDictWindowSubscriber>
           .RaiseEvent<IGridSelectingStateHandler>(h => {
                h?.OnSelecting(new TaskCompletionSource<Component>(), _requesterType);
            });
        return Task.CompletedTask;
    }

    public void OnGetResultOfSelection(Component component, Type requesterType) {
        if (requesterType != _requesterType)
            return;
        if (_butTcs is null)
            return;
        
        new WindowContainer<NomDictWindow>().RuntimeParam.Hide();
        
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
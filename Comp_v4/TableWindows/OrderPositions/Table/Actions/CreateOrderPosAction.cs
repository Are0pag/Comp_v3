using Comp_v4._Installers;
using Comp_v4.Entry.Vm.Buts;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.View;
using Comp_v4.TableWindows.OrderPositions.Form.Actions;
using Comp_v4.TableWindows.OrderPositions.Form.Entities;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Table.Actions;

public class CreateOrderPosAction : BaseActionAsyncSelfWaiting, IGetResultOfSelectionHanlder
{
    protected readonly NomDictButVm _nomDictButVm;
    protected readonly IServiceProvider _serviceProvider;
    
    protected TaskCompletionSource? _butTcs;
    protected Type? _requesterType;
    
    public CreateOrderPosAction(CreateOrderPosFormButVm button, NomDictButVm nomDictButVm, IServiceProvider serviceProvider) : base(button) {
        _nomDictButVm = nomDictButVm;
        _serviceProvider = serviceProvider;
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

        var form = _serviceProvider.GetRequiredService<OpForm>();
        _ = form.ChangeState(form.GetState<CreateOpFormState>(), form);
        
        new InstanceContainer<NomDictWindow>().RuntimeParam.Hide();

        if (new InstanceContainer<SoDataGridVm>().RuntimeParam.LastSelectedSupplierOrder is not { } lastSelectedSo) {
            Console.Error.WriteLine($"Supplier order positions are not give a value");
            throw new InvalidOperationException();
        }
        
        _ = form.Save(new TaskCompletionSource(), new OrderPosition() {
            Position = component, 
            SupplierOrder = lastSelectedSo
        });

        _butTcs.SetResult();
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }
}
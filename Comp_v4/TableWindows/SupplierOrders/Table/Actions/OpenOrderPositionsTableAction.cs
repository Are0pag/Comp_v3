using Comp_v4.TableWindows.OrderPositions.Events;
using Comp_v4.TableWindows.OrderPositions.Table;
using Comp_v4.TableWindows.OrderPositions.Table.Actions;
using Comp_v4.TableWindows.OrderPositions.Table.Entities;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class OpenOrderPositionsTableAction : BaseActionAsyncSelfWaiting
{
    protected readonly SoDataGridVm _gridVm;
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    
    public OpenOrderPositionsTableAction(OpenOrderPositionsButVm button, SoDataGridVm gridVm, IServiceProvider serviceProvider, IWindowOrderLocator windowOrderLocator) : base(button) {
        _gridVm = gridVm;
        _serviceProvider = serviceProvider;
        _windowOrderLocator = windowOrderLocator;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        if (_gridVm.SelectedItem is not {} item)
            throw new ApplicationException("Selected item is not an item");

        _currentTcs = tcs;
        var window = _serviceProvider.GetRequiredService<OrderPositionsTableWindow>();
        var parent = new WindowContainer<SupplierOrderTableWindow>().RuntimeParam;
        window.Owner = parent;
        _windowOrderLocator.RegisterWindow(window);
        window.Closed += async (sender, args) => {
            _windowOrderLocator.UnregisterWindow(window);
            tcs.TrySetResult();
        };

        _serviceProvider.GetRequiredService<CreateOrderPosAction>();
        _serviceProvider.GetRequiredService<EditOrderPosAction>();
        
        EventBus<IOrderPositionSubscriber>.RaiseEvent<IOpTableReloadHandler>(h => h?.OnOpTableReload());
        
        WindowService.BindChildToParent(parent, window);
        window.Show();
        await tcs.Task;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _gridVm.SelectedItem != null;
    }
}
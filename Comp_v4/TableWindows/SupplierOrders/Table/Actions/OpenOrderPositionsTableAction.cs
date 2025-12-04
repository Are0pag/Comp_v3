using Comp_v4.TableWindows.OrderPositions.Table;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class OpenOrderPositionsTableAction : BaseActionAsyncSelfWaiting
{
    protected readonly SoDataGridVm _gridVm;
    protected readonly IServiceProvider _serviceProvider;
    
    public OpenOrderPositionsTableAction(OpenOrderPositionsButVm button, SoDataGridVm gridVm, IServiceProvider serviceProvider) : base(button) {
        _gridVm = gridVm;
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        if (_gridVm.SelectedItem is not {} item)
            throw new ApplicationException("Selected item is not an item");

        _currentTcs = tcs;
        var window = _serviceProvider.GetRequiredService<OrderPositionsTableWindow>();
        window.Closed += async (sender, args) => {
            tcs.TrySetResult();
        };
        
        window.Show();
        await tcs.Task;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _gridVm.SelectedItem != null;
    }
}
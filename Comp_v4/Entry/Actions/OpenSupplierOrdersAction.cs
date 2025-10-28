using Comp_v4.Entry.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Comp_v4.TableWindows.SupplierOrders.Table._Installers;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseActionAsyncCompletion
{
    protected readonly IServiceScopeFactory _scopeFactory;
    protected TaskCompletionSource? _currentTcs;
    public OpenSupplierOrdersAction(OrdersButVm button, IServiceScopeFactory scopeFactory) : base(button) {
        _scopeFactory = scopeFactory;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
            scope.ServiceProvider.GetRequiredService<AddSoAction>();
            window.Closed += (_, __) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
            await tcs.Task;
        }
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}
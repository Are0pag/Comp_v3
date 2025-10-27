using Comp_v4.Entry.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseActionAsyncCompletion
{
    protected readonly IServiceScopeFactory _scopeFactory;
    public OpenSupplierOrdersAction(OrdersButVm button, IServiceScopeFactory scopeFactory) : base(button) {
        _scopeFactory = scopeFactory;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
            window.Closed += (_, __) => tcs.TrySetResult();
            await tcs.Task;
        }
    }

    public override bool CanPerform() {
        return true;
    }
}
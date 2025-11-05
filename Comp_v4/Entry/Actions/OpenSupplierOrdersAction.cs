using Comp_v4.Entry.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseActionAsyncCompletion
{
    protected readonly IServiceProvider _serviceProvider;
    protected TaskCompletionSource? _currentTcs;
    public OpenSupplierOrdersAction(OrdersButVm button, IServiceProvider serviceProvider) : base(button) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        var window = _serviceProvider.GetRequiredService<SupplierOrderTableWindow>();
        _serviceProvider.GetRequiredService<AddSoAction>();
        _serviceProvider.GetRequiredService<EditSoAction>();
        window.Closed += (_, __) => {
            _currentTcs.TrySetResult();
        };
        window.Show();
        await tcs.Task;
        
        /*using (var scope = _serviceProvider.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
            scope.ServiceProvider.GetRequiredService<AddSoAction>().ParentScope = scope;
            scope.ServiceProvider.GetRequiredService<EditSoAction>().ParentScope = scope;
            window.Closed += (_, __) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
            await tcs.Task;
        }*/
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}
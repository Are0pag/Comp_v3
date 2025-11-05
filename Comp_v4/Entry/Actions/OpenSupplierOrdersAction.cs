using Comp_v4.Entry.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseActionAsyncScopeHandler
{
    public OpenSupplierOrdersAction(OrdersButVm button, IServiceScopeFactory scopeFactory) 
        : base(button, scopeFactory) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;

        _currentScope = _scopeFactory.CreateScope();
        
        var window = _currentScope.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
        _currentScope.ServiceProvider.GetRequiredService<AddSoAction>();
        _currentScope.ServiceProvider.GetRequiredService<EditSoAction>();
        window.Closed += OnWindowClosed;
        window.OnReload += async () => {
            window.Close();
            await _button.OnClickAsync();
        };
        window.Show();
        await tcs.Task;
    }
}
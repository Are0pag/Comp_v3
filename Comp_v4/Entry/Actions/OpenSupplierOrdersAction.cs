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
        //Console.WriteLine("1. Perform started");
        _currentTcs = tcs;

        _currentScope = _scopeFactory.CreateScope();
        
        var window = _currentScope.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
        _currentScope.ServiceProvider.GetRequiredService<AddSoAction>();
        _currentScope.ServiceProvider.GetRequiredService<EditSoAction>();
        
        window.Closed += OnWindowClosed;
        
        window.OnReload += async () => {
            //Console.WriteLine("3. OnReload started");
            window.Close();
            
            await Task.Delay(1);
            //Console.WriteLine("6. After window.Close()");
            
            await _button.OnClickAsync();
            //Console.WriteLine("7. OnReload completed");
        };

        window.Show();
        //Console.WriteLine("2. Before await tcs.Task");
        
        await tcs.Task;
        //Console.WriteLine("5.1 After await tcs.Task - Perform completed");
    }
}
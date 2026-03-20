using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class AddSoAction : BaseActionAsyncScopeHandler
{
    protected readonly SupplierOrderTableWindow _supplierOrderTableWindow;
    public AddSoAction(AddSoButVm button, IServiceScopeFactory scopeFactory, SupplierOrderTableWindow supplierOrderTableWindow) 
        : base(button, scopeFactory) {
        _supplierOrderTableWindow = supplierOrderTableWindow;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            
            scope.ServiceProvider.GetRequiredService<SoForm>();
            scope.ServiceProvider.GetRequiredService<SaveFormAction>();
            scope.ServiceProvider.GetRequiredService<CounterpartySelectAction>();
            
            scope.ServiceProvider.GetRequiredService<SetContractLinkAction>();
            scope.ServiceProvider.GetRequiredService<SetInvoiceLinkAction>();
            
            window.Closed += async (sender, args) => {
                _currentTcs.TrySetResult();
                await Task.Delay(AppConfig.TCS_EXECUTION_DELAY);
                _supplierOrderTableWindow.OnReload?.Invoke();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }
}
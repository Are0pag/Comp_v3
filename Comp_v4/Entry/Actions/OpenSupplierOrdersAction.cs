using System.Windows;
using Comp_v4.Entry.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseAsyncActionScopeReloadable
{
    public OpenSupplierOrdersAction(OrdersButVm button, IServiceScopeFactory scopeFactory) 
        : base(button, scopeFactory) {
    }

    protected override Window GetWindow() {
        return _currentScope!.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
    }

    protected override void InstantiateRelatedServices() {
        _currentScope!.ServiceProvider.GetRequiredService<AddSoAction>();
        _currentScope.ServiceProvider.GetRequiredService<EditSoAction>();
    }
}
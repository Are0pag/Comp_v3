using System.Windows;
using Comp_v4.Entry.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.WPF;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseAsyncActionScopeReloadable
{
    protected readonly IWindowOrderLocator _windowOrderLocator;
    public OpenSupplierOrdersAction(OrdersButVm button, IServiceScopeFactory scopeFactory, IWindowOrderLocator windowOrderLocator) 
        : base(button, scopeFactory) {
        _windowOrderLocator = windowOrderLocator;
    }

    protected override Window GetWindow() {
        var supplierOrderTableWindow = _currentScope!.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
        
        _windowOrderLocator.RegisterWindow(supplierOrderTableWindow);
        supplierOrderTableWindow.Closed += (sender, args) => {
            _windowOrderLocator.UnregisterWindow(supplierOrderTableWindow);
        };
        return supplierOrderTableWindow;
    }

    protected override void InstantiateRelatedServices() {
        _currentScope!.ServiceProvider.GetRequiredService<AddSoAction>();
        _currentScope.ServiceProvider.GetRequiredService<EditSoAction>();
        _currentScope.ServiceProvider.GetRequiredService<DeleteSoAction>();
        
        _currentScope.ServiceProvider.GetRequiredService<OpenOrderPositionsTableAction>();
        _currentScope.ServiceProvider.GetRequiredService<OpenPaymentOrderTableAction>();
    }
}
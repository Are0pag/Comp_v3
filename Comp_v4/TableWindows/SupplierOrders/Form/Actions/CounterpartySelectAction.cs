using System.Windows;
using Comp_v4.TableWindows.Counterparties.Table;
using Comp_v4.TableWindows.Counterparties.Table.Actions;
using Comp_v4.TableWindows.Counterparties.Table.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class CounterpartySelectAction : BaseAsyncActionScopeReloadable
{
    public CounterpartySelectAction(CounterpartySelectButVm button, IServiceScopeFactory scopeFactory) : base(button, scopeFactory) {
    }

    protected override Window GetWindow() {
        var window = _currentScope!.ServiceProvider.GetRequiredService<CounterpartyTableWindow>();
        var parent = new WindowContainer<SupplierOrderFormWindow>().RuntimeParam;
        window.Owner = parent;
        WindowService.BindChildToParent(parent, window);
        return window;
    }

    protected override void InstantiateRelatedServices() {
        _currentScope!.ServiceProvider.GetRequiredService<TableCounterparty>();
        
        _currentScope.ServiceProvider.GetRequiredService<AddCounterpartyAction>();
        _currentScope.ServiceProvider.GetRequiredService<EditCounterpartyAction>();
        _currentScope.ServiceProvider.GetRequiredService<DeleteCounterpartyAction>();
        
        _currentScope.ServiceProvider.GetRequiredService<ConfirmSelectionAction>();
    }
}
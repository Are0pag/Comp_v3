using System.Windows;
using Comp_v4._Installers;
using Comp_v4.Entry.Vm.Buts;
using Comp_v4.NomDict.View;
using Comp_v4.TableWindows.OrderPositions.Table.Actions;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseAsyncActionScopeReloadable, IRuntimeParamsContainer<EntryWindow>
{
    protected readonly IWindowOrderLocator _windowOrderLocator;
    protected readonly IServiceProvider _serviceProvider;
    protected EntryWindow _item;
    public OpenSupplierOrdersAction(OrdersButVm button, IServiceScopeFactory scopeFactory, IWindowOrderLocator windowOrderLocator, IServiceProvider serviceProvider) 
        : base(button, scopeFactory) {
        _windowOrderLocator = windowOrderLocator;
        _serviceProvider = serviceProvider;
    }

    protected override Window GetWindow() {
        var supplierOrderTableWindow = _currentScope!.ServiceProvider.GetRequiredService<SupplierOrderTableWindow>();
        supplierOrderTableWindow.Owner = RuntimeParam;
        WindowService.BindChildToParent(RuntimeParam, supplierOrderTableWindow);
        
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

        VeryBagPractice();
    }

    private void VeryBagPractice() {
        var soDg = _currentScope!.ServiceProvider.GetRequiredService<SoDataGridVm>();
        _serviceProvider.GetRequiredService<CreateOrderPosAction>().SoDataGridVm = soDg;
        _serviceProvider.GetRequiredService<EditOrderPosAction>().SoDataGridVm = soDg;
        _serviceProvider.GetRequiredService<OpDataGridVm>().SoDataGridVm = soDg;
    }
    
    public EntryWindow RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<EntryWindow>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _item;
        }
        set => _item = value;
    }
}
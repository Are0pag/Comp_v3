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
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseActionAsyncCompletion 
{
    protected readonly IServiceProvider _serviceProvider;
    protected TaskCompletionSource? _currentTcs;
    public OpenSupplierOrdersAction(OrdersButVm  button, IServiceProvider serviceProvider) : base(button) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        
        var window = _serviceProvider.GetRequiredService<SupplierOrderTableWindow>();
        var parent = new InstanceContainer<EntryWindow>().RuntimeParam;
        window.Owner = parent;
        WindowService.SetMovingAreaInsideParent(parent, window);

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };

        _serviceProvider.GetRequiredService<AddSoAction>();
        _serviceProvider.GetRequiredService<EditSoAction>();
        _serviceProvider.GetRequiredService<DeleteSoAction>();
        _serviceProvider.GetRequiredService<OpenPaymentOrderTableAction>();
        _serviceProvider.GetRequiredService<OpenOrderPositionsTableAction>();

        var so = _serviceProvider.GetRequiredService<SoDataGridVm>();
        _serviceProvider.GetRequiredService<EditOrderPosAction>().SoDataGridVm = so;
        _serviceProvider.GetRequiredService<OpDataGridVm>().SoDataGridVm = so;
        
        window.Show();
        await _currentTcs.Task;
        _currentTcs = null;
    }

    public override bool CanPerform() {
        return _currentTcs == null;
    }
}
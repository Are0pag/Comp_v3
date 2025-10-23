using Comp_v4.Entry.Vm.Buts;
using Comp_v4.Installers;
using Comp_v4.TableWindows.SupplierOrders.Table;
using DI;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Actions;

public class OpenSupplierOrdersAction : BaseActionAsyncCompletion
{
    protected readonly SupplierOrderTableContainer _container;
    public OpenSupplierOrdersAction(OrdersButVm button, SupplierOrderTableContainer container) : base(button) {
        _container = container;
    }

    public override Task Perform(TaskCompletionSource tcs) {
        WindowContextResolver.ResolveWindow<SupplierOrderTableWindow>(_container);
        return Task.CompletedTask;
    }

    public override bool CanPerform() {
        return true;
    }
}
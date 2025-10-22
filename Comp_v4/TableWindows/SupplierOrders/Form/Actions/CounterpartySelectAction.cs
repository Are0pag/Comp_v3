using Comp_v4.Installers;
using Comp_v4.TableWindows.Counterparties.Table;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using DI;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class CounterpartySelectAction : BaseActionAsyncCompletion
{
    protected readonly CounterpartyTableContainer _container;
    public CounterpartySelectAction(CounterpartySelectButVm button, CounterpartyTableContainer container) : base(button) {
        _container = container;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        WindowContextResolver.ResolveWindow<CounterpartyTableWindow>(_container);
    }

    public override bool CanPerform() {
        return true;
    }
}
using Comp_v4.TableWindows.PaymentOrders.Table;
using Comp_v4.TableWindows.PaymentOrders.Table.Actions;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class OpenPaymentOrderTableAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceProvider _serviceProvider;
    public OpenPaymentOrderTableAction(OpenPaymentOrdersButVm button, IServiceProvider serviceProvider) : base(button) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var window = _serviceProvider.GetRequiredService<PaymentOrdersTableWindow>();

        ResolveRelated();

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
    }

    private void ResolveRelated() {
        _serviceProvider.GetRequiredService<AddPoAction>();
    }
}
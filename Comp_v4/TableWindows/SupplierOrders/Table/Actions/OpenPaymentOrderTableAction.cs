using Comp_v4.TableWindows.PaymentOrders.Table;
using Comp_v4.TableWindows.PaymentOrders.Table.Actions;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class OpenPaymentOrderTableAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly SoDataGridVm _soDataGridVm;
    public OpenPaymentOrderTableAction(OpenPaymentOrdersButVm button, IServiceProvider serviceProvider, SoDataGridVm soDataGridVm) : base(button) {
        _serviceProvider = serviceProvider;
        _soDataGridVm = soDataGridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var window = _serviceProvider.GetRequiredService<PaymentOrdersTableWindow>();
        var parent = new InstanceContainer<SupplierOrderTableWindow>().RuntimeParam;
        window.Owner = parent;

        ResolveRelated();

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        WindowService.BindChildToParent(parent, window);
        window.Show();
    }

    private void ResolveRelated() {
        _serviceProvider.GetRequiredService<AddPoAction>().CurrentSo = _soDataGridVm.SelectedItem;
        _serviceProvider.GetRequiredService<EditPoAction>();
        _serviceProvider.GetRequiredService<PaymentOrdersGridVm>().SoDataGridVm = _soDataGridVm;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _soDataGridVm.SelectedItem != null;
    }
}
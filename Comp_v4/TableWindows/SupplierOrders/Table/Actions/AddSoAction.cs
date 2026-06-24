using Comp_v4.Entry;
using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class AddSoAction : BaseActionAsyncCompletion
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly SoDataGridVm _soDataGridVm;
    protected TaskCompletionSource? _currentTcs;
    
    public AddSoAction(AddSoButVm  button, IServiceProvider serviceProvider, SoDataGridVm soDataGridVm) : base(button) {
        _serviceProvider = serviceProvider;
        _soDataGridVm = soDataGridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        if (_soDataGridVm.SelectedItem is not { } so) {
            throw new NullReferenceException();
        }

        var window = ActivatorUtilities.CreateInstance<SupplierOrderFormWindow>(_serviceProvider, so);
        window.Owner = new InstanceContainer<SupplierOrderTableWindow>().RuntimeParam;
        WindowService.SetMovingAreaInsideParent(_serviceProvider.GetRequiredService<EntryWindow>(), window);
        window.Closed += (sender, args) => {
            _currentTcs.TrySetResult();
        };
        
        _serviceProvider.GetRequiredService<SoForm>();
        _serviceProvider.GetRequiredService<SaveFormAction>();
        _serviceProvider.GetRequiredService<CancelFormAction>().SetCash(so);
        _serviceProvider.GetRequiredService<CounterpartySelectAction>();
            
        _serviceProvider.GetRequiredService<SetContractLinkAction>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<SetInvoiceLinkAction>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<OrderStatusEnumsVm>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<VatStatusEnumVm>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<ResetOrderDateButVm>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<ResetDeliveryDateButVm>().SupplierOrder = so;
        
        window.Show();
        await _currentTcs.Task;
        _currentTcs = null;
    }

    public override bool CanPerform() {
        return _currentTcs == null;
    }
}
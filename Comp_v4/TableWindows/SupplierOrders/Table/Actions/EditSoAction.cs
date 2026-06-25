using System.Windows;
using Comp_v4.Entry;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class EditSoAction : BaseActionAsyncCompletion
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly SoDataGridVm _soDataGridVm;
    protected TaskCompletionSource? _currentTcs;
    
    public EditSoAction(EditSoButVm  button, IServiceProvider serviceProvider, SoDataGridVm soDataGridVm) : base(button) {
        _serviceProvider = serviceProvider;
        _soDataGridVm = soDataGridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        if (_soDataGridVm.SelectedItem is not { } so) {
            throw new NullReferenceException();
        }

        Window window;
        try {
            window = ActivatorUtilities.CreateInstance<SupplierOrderFormWindow>(_serviceProvider, so);
        }
        catch (Exception ex) {
            MessageBox.Show(ex.ToString());
            throw;
        }

        WindowServiceHelper.Register<EntryWindow, SupplierOrderTableWindow>(window);
        window.Closed += (sender, args) => {
            _currentTcs.TrySetResult();
        };

        SoForm soForm;
        try {
            soForm = _serviceProvider.GetRequiredService<SoForm>();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        await soForm.ChangeState(soForm.GetState<EditSoFormState>(), soForm);

        try {
            _serviceProvider.GetRequiredService<SaveFormAction>();
            _serviceProvider.GetRequiredService<CancelFormAction>().SetCash(so);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        _serviceProvider.GetRequiredService<CounterpartySelectAction>();

        _serviceProvider.GetRequiredService<SetContractLinkAction>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<SetInvoiceLinkAction>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<OrderStatusEnumsVm>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<VatStatusEnumVm>().SupplierOrder = so;
        _serviceProvider.GetRequiredService<ResetOrderDateButVm>().SupplierOrder = so;

        _serviceProvider.GetRequiredService<ResetDeliveryDateButVm>().SupplierOrder = so;
        
        var tasks = new List<Task>();
        EventBus<ISupplierOrdersSubscriber>.RaiseEvent<ICreateSupplierOrdersHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);
    
            try {
                h?.OnCreateSupplierOrder(subscriberTcs);
            }
            catch (Exception ex) {
                subscriberTcs.TrySetException(ex);
            }
        });
        await Task.WhenAll(tasks);
        
        
        
        window.Show();
        await _currentTcs.Task;
        _currentTcs = null;
    }

    public override bool CanPerform() {
        return _currentTcs == null && _soDataGridVm.SelectedItem != null;
    }
}
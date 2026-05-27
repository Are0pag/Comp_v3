using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class EditSoAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceScopeFactory _scopeFactory;
    protected readonly SupplierOrderTableWindow _supplierOrderTableWindow;
    protected readonly SoDataGridVm _soDataGridVm;
    
    public EditSoAction(EditSoButVm button, 
                        IServiceScopeFactory scopeFactory, 
                        SupplierOrderTableWindow supplierOrderTableWindow,
                        SoDataGridVm soDataGridVm) 
        : base(button) {
        _scopeFactory = scopeFactory;
        _supplierOrderTableWindow = supplierOrderTableWindow;
        _soDataGridVm = soDataGridVm;
    }
    
    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            var parent = new WindowContainer<SupplierOrderTableWindow>().RuntimeParam;
            window.Owner = parent;
            
            var soForm = scope.ServiceProvider.GetRequiredService<SoForm>();
            await soForm.ChangeState(soForm.GetState<EditSoFormState>(), soForm);
            
            scope.ServiceProvider.GetRequiredService<SaveFormAction>();
            scope.ServiceProvider.GetRequiredService<CounterpartySelectAction>();
            
            scope.ServiceProvider.GetRequiredService<SetContractLinkAction>();
            scope.ServiceProvider.GetRequiredService<SetInvoiceLinkAction>();

            var item = scope.ServiceProvider.GetRequiredService<SupplierOrder>();
            _soDataGridVm.SelectedItem!.CopyTo(item);
            
            
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
            

            window.Closed += async (sender, args) => {
                _currentTcs.TrySetResult();
                await Task.Delay(AppConfig.TCS_EXECUTION_DELAY);
                _supplierOrderTableWindow.OnReload?.Invoke();
            };
            WindowService.BindChildToParent(parent, window);
            window.Show();
        
            await _currentTcs.Task;
        }
    }

    public override bool CanPerform() {
        return base.CanPerform() && _soDataGridVm?.SelectedItem != null;
    }
}
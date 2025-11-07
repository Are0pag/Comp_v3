using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class EditSoAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceScopeFactory _scopeFactory;
    protected readonly SupplierOrder _supplierOrder;
    protected SoDataGridVm? _soDataGridVm;
    public EditSoAction(EditSoButVm button, IServiceScopeFactory scopeFactory, SupplierOrder supplierOrder) : base(button) {
        _scopeFactory = scopeFactory;
        _supplierOrder = supplierOrder;
    }
    
    public IServiceScope? ParentScope { get; set; }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            
            var soForm = scope.ServiceProvider.GetRequiredService<SoForm>();
            await soForm.ChangeState(soForm.GetState<EditSoFormState>(), soForm);
            
            scope.ServiceProvider.GetRequiredService<SaveFormAction>();
            scope.ServiceProvider.GetRequiredService<CounterpartySelectAction>();
            
            scope.ServiceProvider.GetRequiredService<SetContractLinkAction>();
            scope.ServiceProvider.GetRequiredService<SetInvoiceLinkAction>();
            
            if (ParentScope?.ServiceProvider.GetRequiredService<SoDataGridVm>() is not { } dg)
                throw new NullReferenceException("EditSoAction");
            dg.SelectedItem!.CopyTo(_supplierOrder);
            _soDataGridVm = dg;

            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }

    public override bool CanPerform() {
        return base.CanPerform() && _soDataGridVm?.SelectedItem != null;
    }
}
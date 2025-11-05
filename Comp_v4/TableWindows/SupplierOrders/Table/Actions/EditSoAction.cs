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
    protected readonly IServiceProvider _serviceProvider;
    protected readonly SupplierOrder _supplierOrder;
    protected SoDataGridVm? _soDataGridVm;
    public EditSoAction(EditSoButVm button, IServiceProvider serviceProvider, SupplierOrder supplierOrder) : base(button) {
        _serviceProvider = serviceProvider;
        _supplierOrder = supplierOrder;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        
        var window = _serviceProvider.GetRequiredService<SupplierOrderFormWindow>();
            
        var soForm = _serviceProvider.GetRequiredService<SoForm>();
        await soForm.ChangeState(soForm.GetState<EditSoFormState>(), soForm);
            
        _serviceProvider.GetRequiredService<SaveFormAction>();
        _serviceProvider.GetRequiredService<CounterpartySelectAction>();
            
        if (_serviceProvider.GetRequiredService<SoDataGridVm>() is not { } dg)
            throw new NullReferenceException("EditSoAction");
        dg.SelectedItem!.CopyTo(_supplierOrder);
        _soDataGridVm = dg;

        window.Closed += (sender, args) => {
            _currentTcs.TrySetResult();
        };
        window.Show();
        
        await _currentTcs.Task;
        
        /*using (var scope = _serviceProvider.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            
            var soForm = scope.ServiceProvider.GetRequiredService<SoForm>();
            await soForm.ChangeState(soForm.GetState<EditSoFormState>(), soForm);
            
            scope.ServiceProvider.GetRequiredService<SaveFormAction>();
            scope.ServiceProvider.GetRequiredService<CounterpartySelectAction>().ParentScope = scope;
            
            if (ParentScope?.ServiceProvider.GetRequiredService<SoDataGridVm>() is not { } dg)
                throw new NullReferenceException("EditSoAction");
            dg.SelectedItem!.CopyTo(_supplierOrder);
            _soDataGridVm = dg;

            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }*/
    }

    public override bool CanPerform() {
        return base.CanPerform() && _soDataGridVm?.SelectedItem != null;
    }
}
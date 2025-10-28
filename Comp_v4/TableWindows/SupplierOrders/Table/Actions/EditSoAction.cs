using Comp_v4.TableWindows.SupplierOrders.Form;
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
    public EditSoAction(EditSoButVm button, IServiceScopeFactory scopeFactory, SupplierOrder supplierOrder) : base(button) {
        _scopeFactory = scopeFactory;
        _supplierOrder = supplierOrder;
    }
    
    public IServiceScope? ParentScope { get; set; }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            
            if (ParentScope?.ServiceProvider.GetRequiredService<SoDataGridVm>().SelectedItem is not { } selectedSo)
                throw new NullReferenceException("EditSoAction");
            selectedSo.CopyTo(_supplierOrder);
            
            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }
}
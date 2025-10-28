using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Installers;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class AddSoAction : BaseActionAsyncCompletion
{
    protected readonly IServiceScopeFactory _scopeFactory;
    protected TaskCompletionSource? _currentTcs;
    public AddSoAction(AddSoButVm button, IServiceScopeFactory scopeFactory) : base(button) {
        _scopeFactory = scopeFactory;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            
            scope.ServiceProvider.GetRequiredService<SoForm>();
            scope.ServiceProvider.GetRequiredService<SaveFormAction>();
            
            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}
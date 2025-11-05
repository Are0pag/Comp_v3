using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class AddSoAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceProvider _serviceProvider;

    public AddSoAction(AddSoButVm button, IServiceProvider serviceProvider) : base(button) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        
        var window = _serviceProvider.GetRequiredService<SupplierOrderFormWindow>();
        
        _serviceProvider.GetRequiredService<SoForm>();
        _serviceProvider.GetRequiredService<SaveFormAction>();
        _serviceProvider.GetRequiredService<CounterpartySelectAction>();
            
        window.Closed += (sender, args) => {
            _currentTcs.TrySetResult();
        };
        window.Show();
        
        await _currentTcs.Task;
        /*using (var scope = _serviceProvider.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            
            scope.ServiceProvider.GetRequiredService<SoForm>();
            scope.ServiceProvider.GetRequiredService<SaveFormAction>();
            scope.ServiceProvider.GetRequiredService<CounterpartySelectAction>().ParentScope = scope;
            
            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }*/
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}
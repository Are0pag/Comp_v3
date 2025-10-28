using Comp_v4.TableWindows.SupplierOrders.Form;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class EditSoAction : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceScopeFactory _scopeFactory;
    public EditSoAction(BaseButtonAdvanced button, IServiceScopeFactory scopeFactory) : base(button) {
        _scopeFactory = scopeFactory;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }
    }
}
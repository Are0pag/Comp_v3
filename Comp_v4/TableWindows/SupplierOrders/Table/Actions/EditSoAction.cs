using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class EditSoAction : BaseActionAsyncSelfWaiting
{
    public EditSoAction(BaseButtonAdvanced button) : base(button) {
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        /*_currentTcs = tcs;
        using (var scope = _scopeFactory.CreateScope()) {
            //var window = scope.ServiceProvider.GetRequiredService<ISupplierOrderFormWindowFactory>().Create(new SupplierOrder());
            // и тут надо будет перезаписать но как получить ссылку на IServiceCollection?
            var window = scope.ServiceProvider.GetRequiredService<SupplierOrderFormWindow>();
            window.Closed += (sender, args) => {
                _currentTcs.TrySetResult();
            };
            window.Show();
        
            await _currentTcs.Task;
        }*/
    }
}
using Comp_v4.TableWindows.SupplierOrders.Table._Installers;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class AddSoAction : BaseActionAsyncCompletion
{
    protected readonly ISupplierOrderFormWindowFactory _formHandler;
    protected TaskCompletionSource? _currentTcs;
    public AddSoAction(AddSoButVm button, ISupplierOrderFormWindowFactory formHandler) : base(button) {
        _formHandler = formHandler;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;

        var window = _formHandler.Create(new SupplierOrder());
        window.Closed += (sender, args) => {
            _currentTcs.TrySetResult();
        };
        window.Show();
        
        await _currentTcs.Task;
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}
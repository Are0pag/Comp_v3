using System.Windows;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.WPF.Buttons;
using Utils.WPF.Dialogs;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Actions;

public class DeletePoAction : BaseActionAsyncSelfWaiting
{
    protected readonly PaymentOrdersGridVm _gridVm;
    protected readonly IRepository<PaymentOrder> _repository;
    public DeletePoAction(DeletePaymentOrderButVm button, PaymentOrdersGridVm gridVm, IRepository<PaymentOrder> repository) : base(button) {
        _gridVm = gridVm;
        _repository = repository;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        if (_gridVm.SelectedItem is not { } op) {
            Console.Error.WriteLine("Selected op is not selected");
            throw new NullReferenceException("Selected op is not null");
        }

        // if (await _repository.HasAnyUsagesAsync<SupplierOrder, PaymentOrder>(op)) {
        //     MessageBox.Show("Невозможно удалить элемент, так как он ещё используется");
        //     return;
        // }
        
        var isConfirmed = DialogService.ShowConfirmation("Удаление", $"Вы уверены что хотите удалить позицию?");
        if (!isConfirmed) return;

        try {
            await _repository.DeleteAsync(op.Id);
        }
        catch (Exception e) {
            Console.Error.WriteLine(e);
            throw;
        }

        if (!_gridVm.RemoveItem(op)) {
            Console.Error.WriteLine("Failed to delete order position");
            throw new NullReferenceException("Failed to delete order position");
        }

        tcs.TrySetResult();
    }

    public override bool CanPerform() {
        return base.CanPerform() && _gridVm.SelectedItem != null;
    }
}
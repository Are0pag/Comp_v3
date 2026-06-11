using System.Windows;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.WPF.Buttons;
using Utils.WPF.Dialogs;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Actions;

public class DeleteSoAction : BaseActionAsyncCompletion
{
    protected readonly SoDataGridVm _soDataGridVm;
    protected readonly IRepository<SupplierOrder> _repository;
    protected readonly SupplierOrderTableWindow _tableWindow;
    
    public DeleteSoAction(DeleteSoButVm button, SoDataGridVm soDataGridVm, IRepository<SupplierOrder> repository, SupplierOrderTableWindow tableWindow) : base(button) {
        _soDataGridVm = soDataGridVm;
        _repository = repository;
        _tableWindow = tableWindow;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var so = _soDataGridVm.SelectedItem!;

        if (await _repository.HasAnyUsagesAsync<OrderPosition, SupplierOrder>(so) ||
            await _repository.HasAnyUsagesAsync<PaymentOrder, SupplierOrder>(so)) {
            MessageBox.Show("Невозможно удалить элемент, так как он ещё используется");
            return;
        }
        
        var isConfirmed = DialogService.ShowConfirmation("Удаление", $"Вы уверены что хотите удалить позицию?");
        if (!isConfirmed) return;
        
        await _repository.DeleteAsync(so.Id);
        tcs.TrySetResult();
        _tableWindow.OnReload?.Invoke();
    }

    public override bool CanPerform() {
        return _soDataGridVm.SelectedItem != null;
    }
}
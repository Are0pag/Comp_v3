using System.Windows;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.WPF.Buttons;
using Utils.WPF.Dialogs;
using DeleteOrderPositionButVm = Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts.DeleteOrderPositionButVm;

namespace Comp_v4.TableWindows.OrderPositions.Table.Actions;

public class DeleteOrderPositionAction : BaseActionAsyncSelfWaiting
{
    protected readonly IRepository<OrderPosition> _repository;
    protected readonly OpDataGridVm _gridVm;
    
    public DeleteOrderPositionAction(DeleteOrderPositionButVm button, IRepository<OrderPosition> repository, OpDataGridVm gridVm) : base(button) {
        _repository = repository;
        _gridVm = gridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        if (_gridVm.SelectedItem is not { } op) {
            Console.Error.WriteLine("Selected op is not selected");
            throw new NullReferenceException("Selected op is not null");
        }
        
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
    }

    public override bool CanPerform() {
        return base.CanPerform() && _gridVm.SelectedItem != null; // && _repository.Find 
    }
}
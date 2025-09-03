using System.Windows.Input;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgEditing : StateDataGrid
{
    /* То есть пользователь нажимает кнопку "Создать" */
    public StateDgEditing(HeterochromicCommandScheduler<IDeferredCommand> scheduler) : base(scheduler) {
    }

    public override Task AddItemAsync(CognDesignGridVm vm) {
       vm.StateProvider.ChangeState(vm.StateProvider.GetState<StateDgCreatingNewItem>(), vm);
       return Task.CompletedTask;
    }

    public override async Task OnHandleKeyInput(CognDesignGridVm vm, object? sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Delete when CanDeleteItem(vm):
                await DeleteItemAsync(vm);
                break;
            case Key.OemPlus when CanAddItem(vm):
            case Key.Add when CanAddItem(vm):
                await AddItemAsync(vm);
                break;
        }
    }
}
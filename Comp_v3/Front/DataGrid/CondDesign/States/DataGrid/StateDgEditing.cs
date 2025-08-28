
namespace Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;

public class StateDgEditing : StateDataGrid
{
    /* То есть пользователь нажимает кнопку "Создать" */
    public override Task AddItemAsync(CognDesignGridVm vm) {
       vm.StateProvider.ChangeState(vm.StateProvider.CreateNewItem);
       vm.StateProvider.CurrentStateDataGrid.Entry(vm);
       return Task.CompletedTask;
    }
}
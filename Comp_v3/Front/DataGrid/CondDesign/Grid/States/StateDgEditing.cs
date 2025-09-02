namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgEditing : StateDataGrid
{
    /* То есть пользователь нажимает кнопку "Создать" */
    public override Task AddItemAsync(CognDesignGridVm vm) {
       vm.StateProvider.ChangeState(vm.StateProvider.GetState<StateDgCreatingNewItem>(), vm);
       return Task.CompletedTask;
    }
}
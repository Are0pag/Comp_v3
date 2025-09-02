using System.Windows.Input;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgEditing : StateDataGrid
{
    /* То есть пользователь нажимает кнопку "Создать" */
    public override Task AddItemAsync(CognDesignGridVm vm) {
       vm.StateProvider.ChangeState(vm.StateProvider.GetState<StateDgCreatingNewItem>(), vm);
       return Task.CompletedTask;
    }
    
    public override void OnHandleKeyInput(CognDesignGridVm vm, object? sender, KeyEventArgs e) {
        base.OnHandleKeyInput(vm, sender, e);
        switch (e.Key) {
            case Key.Delete when CanDeleteItem(vm):
                DeleteItemAsync(vm);
                break;
            case Key.OemPlus when CanAddItem(vm):
            case Key.Add when CanAddItem(vm):
                AddItemAsync(vm);
                break;
        }
    }
}
using System.Windows.Input;
using Infrastructure.Command.Heterochromic;
using WPF.Services.UserActionsHandling.InputKey;
using ICommand = Infrastructure.Command.Base.ICommand;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgEditing : StateDataGrid
{
    public StateDgEditing(HeterochromicCommandScheduler scheduler, CommonUndoRedoHotKeysService commonKeysService) 
        : base(scheduler, commonKeysService) {
    }

    public override Task AddItemAsync(CognDesignGridVm vm) {
       vm.StateProvider.ChangeState(vm.StateProvider.GetState<StateDgCreatingNewItem>(), vm);
       return Task.CompletedTask;
    }

    public override async Task OnHandleKeyInput(CognDesignGridVm vm, object? sender, KeyEventArgs e) {
        await base.OnHandleKeyInput(vm, sender, e);
        if (e.Handled == true) return;
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
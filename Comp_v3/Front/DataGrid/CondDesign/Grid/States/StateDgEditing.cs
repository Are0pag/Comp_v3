using System.Windows.Input;
using Comp_v3.Front.DataGrid.CondDesign.Commands;
using Comp_v3.Front.DataGrid.CondDesign.Transactions;
using Infrastructure.Command.Heterochromic;
using WPF.Services.UserActionsHandling.InputKey;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgEditing : StateDataGrid
{
    public StateDgEditing(HeterochromicCommandScheduler scheduler, CommonUndoRedoHotKeysService commonKeysService) 
        : base(scheduler, commonKeysService) {
    }

    public override async Task AddItemAsync(CognDesignGridVm vm) {
        
        await _scheduler.BeginTransaction<CreateNewRawAndFocusTransaction>("Starts from Vm state Editing")
                        .RegisterCommand(new ChangeTargetVmStateCommand(vm, 
                                                                        this, 
                                                                        vm.StateProvider.GetState<StateDgCreatingNewItem>())
                                 )
                        .ExecuteLastRegisteredAsync();
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
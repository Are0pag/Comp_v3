using System.Windows.Controls;
using System.Windows.Input;
using Comp_v3.Front.DataGrid.CondDesign.Commands;
using Comp_v3.Front.Events.Buttons;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;
using Utils.EventBus;
using WPF.Services.UserActionsHandling.InputKey;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public abstract class StateDataGrid : StateBase<CognDesignGridVm>
{
    protected readonly HeterochromicCommandScheduler _scheduler;
    protected readonly CommonUndoRedoHotKeysService _commonKeysService;

    protected StateDataGrid(HeterochromicCommandScheduler scheduler, CommonUndoRedoHotKeysService commonKeysService) {
        _scheduler = scheduler;
        _commonKeysService = commonKeysService;
    }
    
    public abstract Task AddItemAsync(CognDesignGridVm vm);

    public virtual async Task DeleteItemAsync(CognDesignGridVm vm) {
        if (vm.SelectedItem == null)
            throw new Exception($"Selected item is null when try to delete from {nameof(CognDesignGridVm)}");

        var command = new DeleteItemCommand(vm) { Description = $"myCommand {new Random().Next(1, 1000)}"};
        await _scheduler.ExecuteCommand(command); 
    }

    public virtual async Task OnCellEditEnding(CognDesignGridVm vm, object? sender, DataGridCellEditEndingEventArgs e) {
        //if (CanEditItem(e) && e.Row.Item is ConditionalDesignation cd)
            //await _scheduler.ExecuteCommand(new UpdateItemCommand(vm, cd, e));
    }

    public virtual async Task OnHandleKeyInput(CognDesignGridVm vm, object? sender, KeyEventArgs e) {
        switch (_commonKeysService.HandleInput(e)) {
            
            case ActionType.Undo when _scheduler.CanUndo():
                    await _scheduler.UndoAsync();
                    e.Handled = true;
                    EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute()); 
                break;
            
            case ActionType.Redo when _scheduler.CanRedo():
                    await _scheduler.RedoAsync();
                    e.Handled = true;
                    EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute()); 
                break;
            
            case ActionType.Save when CanSaveChanges():
                await SaveChanges();
                EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
                break;
            
            case ActionType.Cancel: 
                await new CancelEditCommand(vm, sender).ExecuteAsync();
                break;
        }
            
    }

    public virtual async Task SaveChanges() {
        await _scheduler.CommitDeferredChanges();
    }

    public virtual async Task CancelNewItemAdding() {
        await _scheduler.UndoAsync();
    }

#region CanExecute

    public virtual bool CanAddItem(CognDesignGridVm vm) {
        return vm.StateProvider.CurrentState is not StateDgCreatingNewItem;
    }

    public virtual bool CanDeleteItem(CognDesignGridVm vm) {
        return vm.SelectedItem != null;
    }

    public virtual bool CanSaveChanges() {
        return _scheduler.CanUndo();
    }

    protected virtual bool CanEditItem(DataGridCellEditEndingEventArgs e) {
        return e.EditAction == DataGridEditAction.Commit && e.Row.Item is ConditionalDesignation { };
    }

#endregion
}
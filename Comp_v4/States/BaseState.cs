using System.Windows.Controls;
using System.Windows.Input;
using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;
using WPF.Templates.Core;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseState : StateBase<DataGridViewModel>//, IState
{
    protected readonly HeterochromicCommandScheduler _scheduler;

    public BaseState(HeterochromicCommandScheduler scheduler) {
        _scheduler = scheduler;
    }

    public virtual Task AddItemAsync(DataGridViewModel context) {
        return Task.CompletedTask;
    }

    public virtual Task DeleteItemAsync(DataGridViewModel context) {
        return Task.CompletedTask;
    }

    public virtual Task EditItemAsync(DataGridViewModel context) {
        return Task.CompletedTask;
    }
    
    public virtual async Task SaveChanges() {
        await _scheduler.CommitDeferredChanges();
    }

    public virtual Task OnCellEditEnding(DataGridViewModel vm, object? sender, DataGridCellEditEndingEventArgs e) {
        return Task.CompletedTask;
    }

    public virtual async Task OnHandleKeyInput(DataGridViewModel vm, object? sender, KeyEventArgs e) {
        
    }
    
    public virtual async Task CancelNewItemAdding() {
        await _scheduler.UndoAsync();
    }
    
    /*public virtual bool CanAddItem(ViewModel vm) {
        return vm.StateProvider.CurrentState is not StateDgCreatingNewItem;
    }*/
    
    public virtual bool CanAddItem(DataGridViewModel vm) {
        return true; // Базовая реализация, может быть переопределена в наследниках
    }

    public virtual bool CanDeleteItem(DataGridViewModel vm) {
        return vm.SelectedItem != null;
    }

    public virtual bool CanSaveChanges() {
        return _scheduler.CanUndo();
    }

    /*protected virtual bool CanEditItem(DataGridCellEditEndingEventArgs e) {
        return e.EditAction == DataGridEditAction.Commit && e.Row.Item is ConditionalDesignation { };
    }*/
    
    public virtual bool CanEditItem(DataGridCellEditEndingEventArgs e) {
        return true; // Базовая реализация, может быть переопределена в наследниках
    }
}
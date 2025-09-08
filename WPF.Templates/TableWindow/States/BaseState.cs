using System.Windows.Controls;
using System.Windows.Input;
using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;
using WPF.Templates.Core;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseState : StateBase<ViewModel>, IState
{
    protected readonly HeterochromicCommandScheduler _scheduler;

    public BaseState(HeterochromicCommandScheduler scheduler) {
        _scheduler = scheduler;
    }

    public virtual Task AddItemAsync(ViewModel context) {
        return Task.CompletedTask;
    }

    public virtual Task DeleteItemAsync(ViewModel context) {
        return Task.CompletedTask;
    }

    public virtual Task EditItemAsync(ViewModel context) {
        return Task.CompletedTask;
    }
    
    public virtual async Task SaveChanges() {
        await _scheduler.CommitDeferredChanges();
    }

    public virtual Task OnCellEditEnding(ViewModel vm, object? sender, DataGridCellEditEndingEventArgs e) {
        return Task.CompletedTask;
    }

    public virtual async Task OnHandleKeyInput(ViewModel vm, object? sender, KeyEventArgs e) {
        
    }
    
    public virtual async Task CancelNewItemAdding() {
        await _scheduler.UndoAsync();
    }
    
    /*public virtual bool CanAddItem(ViewModel vm) {
        return vm.StateProvider.CurrentState is not StateDgCreatingNewItem;
    }*/
    
    public virtual bool CanAddItem(ViewModel vm) {
        return true; // Базовая реализация, может быть переопределена в наследниках
    }

    public virtual bool CanDeleteItem(ViewModel vm) {
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
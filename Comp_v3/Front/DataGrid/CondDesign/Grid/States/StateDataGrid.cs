using System.Windows.Controls;
using System.Windows.Input;
using Comp_v3.Front.DataGrid.CondDesign.Commands;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public abstract class StateDataGrid : BaseState<CognDesignGridVm>
{
    protected readonly HeterochromicCommandScheduler<IDeferredCommand> _scheduler;

    protected StateDataGrid(HeterochromicCommandScheduler<IDeferredCommand> scheduler) {
        _scheduler = scheduler;
    }

    public abstract Task AddItemAsync(CognDesignGridVm vm);

    public virtual async Task DeleteItemAsync(CognDesignGridVm vm) {
        if (vm.SelectedItem == null)
            throw new Exception($"Selected item is null when try to delete from {nameof(CognDesignGridVm)}");

        var command = new DeleteItemCommand(vm) { Description = $"myCommand {new Random().Next(1, 1000)}"};
        await _scheduler.ExecuteCommand(command); 
    }

    public virtual async Task OnCellEditEnding(CognDesignGridVm vm, object? sender, DataGridCellEditEndingEventArgs e) {
        if (CanEditItem(e))
            await vm.Repository.UpdateAsync((ConditionalDesignation)e.Row.Item);  /* Model Command */
    }

    public virtual async Task OnHandleKeyInput(CognDesignGridVm vm, object? sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Z when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                if (_scheduler.CanUndo) {
                    await _scheduler.UndoAsync();
                    e.Handled = true;
                }
                break;
            case Key.Z when e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift):
            case Key.Y when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                if (_scheduler.CanRedo) {
                    await _scheduler.RedoAsync();
                    e.Handled = true;
                }
                break;
        }
    }

    public virtual async Task SaveChanges() {
        await _scheduler.CommitDeferredChanges();
    }

#region CanExecute

    public virtual bool CanAddItem(CognDesignGridVm vm) {
        return vm.StateProvider.CurrentState is not StateDgCreatingNewItem;
    }

    public virtual bool CanDeleteItem(CognDesignGridVm vm) {
        return vm.SelectedItem != null;
    }

    public virtual bool CanSaveChanges() {
        return _scheduler.CanUndo;
    }

    protected virtual bool CanEditItem(DataGridCellEditEndingEventArgs e) {
        return e.EditAction == DataGridEditAction.Commit && e.Row.Item is ConditionalDesignation { };
    }

#endregion
}
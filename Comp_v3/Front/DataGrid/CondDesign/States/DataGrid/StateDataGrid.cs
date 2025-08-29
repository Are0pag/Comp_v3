using System.Windows.Controls;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Comp.ModelData.TechnicalItems;

namespace Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;

public abstract class StateDataGrid
{
    public virtual void Entry(CognDesignGridVm vm) {}
    
    public abstract Task AddItemAsync(CognDesignGridVm vm);

    public virtual bool CanAddItem(CognDesignGridVm vm) {
        return vm.StateProvider.CurrentStateDataGrid is not StateDgCreatingNewItem;
    }
    
    public virtual async Task DeleteItemAsync(CognDesignGridVm vm) {
        if (vm.SelectedItem == null)
            throw new Exception($"Selected item is null when try to delete from {nameof(CognDesignGridVm)}");
        
        await vm.Repository.DeleteAsync(vm.SelectedItem.Id);
        vm.Items.Remove(vm.SelectedItem);
        vm.SelectedItem = null;
    }

    public virtual bool CanDeleteItem(CognDesignGridVm vm) {
        return vm.SelectedItem != null;
    }

    public virtual async Task OnCellEditEnding(CognDesignGridVm vm, object? sender, DataGridCellEditEndingEventArgs e) {
        if (CanEditItem(e))
            await vm.Repository.UpdateAsync((ConditionalDesignation)e.Row.Item);
    }

    protected virtual bool CanEditItem(DataGridCellEditEndingEventArgs e) {
        return e.EditAction == DataGridEditAction.Commit && e.Row.Item is ConditionalDesignation { };
    }
}
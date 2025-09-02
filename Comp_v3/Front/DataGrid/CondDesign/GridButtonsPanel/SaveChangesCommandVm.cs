using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.DataGrid.CondDesign.Grid;

namespace Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;

public partial class SaveChangesCommandVm : BaseButtonsVm
{
    public SaveChangesCommandVm(CognDesignGridVm condDesignGridVm) : base(condDesignGridVm) {
    }

    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    protected async Task SaveChangesAsync() {
        await _condDesignGridVm.StateProvider.CurrentState.SaveChanges();
    }

    protected bool CanSaveChanges() {
        return true;
    }
}
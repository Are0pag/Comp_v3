using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.DataGrid.CondDesign.Grid;

namespace Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;

public partial class DeleteItemButVm : BaseButtonsVm
{
    public DeleteItemButVm(CognDesignGridVm condDesignGridVm) : base(condDesignGridVm) {
        _condDesignGridVm.PropertyChanged += OnSelectedItemPropertyChanged;
    }

    public override void Dispose() {
        base.Dispose();
        _condDesignGridVm.PropertyChanged -= OnSelectedItemPropertyChanged;
    }
    
    [RelayCommand(CanExecute = nameof(CanDeleteItem))] /* непосредственно через CurrentStateDataGrid.CanDeleteItem не выйдет:( */
    protected async Task DeleteItemAsync() {
        await _condDesignGridVm.StateProvider.CurrentState.DeleteItemAsync(_condDesignGridVm);
    }

    protected bool CanDeleteItem() {
        return _condDesignGridVm.StateProvider.CurrentState.CanDeleteItem(_condDesignGridVm);
    }
    
    protected virtual void OnSelectedItemPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(_condDesignGridVm.SelectedItem)) 
            DeleteItemCommand.NotifyCanExecuteChanged();
    }
}
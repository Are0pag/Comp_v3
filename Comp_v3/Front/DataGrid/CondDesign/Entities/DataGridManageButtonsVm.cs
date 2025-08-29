using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.Events;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Entities;

public partial class DataGridManageButtonsVm : ObservableObject, ICellAddingToDataGridHandler
{
    private readonly IConditionalDesignationRepository _repository;
    private readonly CognDesignGridVm _condDesignGridVm;
    public DataGridManageButtonsVm(IConditionalDesignationRepository repository, CognDesignGridVm condDesignGridVm) {
        _repository = repository;
        _condDesignGridVm = condDesignGridVm;
        _condDesignGridVm.PropertyChanged += OnSelectedItemPropertyChanged;
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
    }
    public virtual void Dispose() {
        _condDesignGridVm.PropertyChanged -= OnSelectedItemPropertyChanged;
        EventBus<IUiGlobalSubscriber>.Unsubscribe(this);
    }

    public void HandleNewValueAdding() {
        AddItemCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    protected async Task AddItem() {
        await _condDesignGridVm.StateProvider.CurrentStateDataGrid.AddItemAsync(_condDesignGridVm);
    }

    protected bool CanAddItem() {
        return _condDesignGridVm.StateProvider.CurrentStateDataGrid.CanAddItem(_condDesignGridVm);
    }

    [RelayCommand(CanExecute = nameof(CanDeleteItem))] /* непосредственно через CurrentStateDataGrid.CanDeleteItem не выйдет:( */
    protected async Task DeleteItemAsync() {
        await _condDesignGridVm.StateProvider.CurrentStateDataGrid.DeleteItemAsync(_condDesignGridVm);
    }

    protected bool CanDeleteItem() {
        return _condDesignGridVm.StateProvider.CurrentStateDataGrid.CanDeleteItem(_condDesignGridVm);
    }
    
    protected virtual void OnSelectedItemPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(_condDesignGridVm.SelectedItem)) 
            DeleteItemCommand.NotifyCanExecuteChanged();
    }
}
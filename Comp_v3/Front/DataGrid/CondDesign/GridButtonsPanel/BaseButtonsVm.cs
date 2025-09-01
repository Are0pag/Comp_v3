using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.Events;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Entities;

public partial class BaseButtonsVm : ObservableObject, ICellAddingToDataGridHandler, ICancelNewItemAddingHandler
{
    private readonly CognDesignGridVm _condDesignGridVm;
    public BaseButtonsVm(CognDesignGridVm condDesignGridVm) {
        _condDesignGridVm = condDesignGridVm;
        _condDesignGridVm.PropertyChanged += OnSelectedItemPropertyChanged;
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
    }
    public virtual void Dispose() {
        _condDesignGridVm.PropertyChanged -= OnSelectedItemPropertyChanged;
        EventBus<IUiGlobalSubscriber>.Unsubscribe(this);
    }
    
    
    void ICancelNewItemAddingHandler.HandleCancelNewItemAdding() {
        AddItemCommand.NotifyCanExecuteChanged();
    }

    void ICellAddingToDataGridHandler.HandleNewValueAdding() {
        AddItemCommand.NotifyCanExecuteChanged();
    }

    
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    protected async Task AddItem() {
        await _condDesignGridVm.StateProvider.CurrentState.AddItemAsync(_condDesignGridVm);
    }

    protected bool CanAddItem() {
        return _condDesignGridVm.StateProvider.CurrentState.CanAddItem(_condDesignGridVm);
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
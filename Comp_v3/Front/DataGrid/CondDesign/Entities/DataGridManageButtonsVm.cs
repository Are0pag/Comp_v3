using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;

namespace Comp_v3.Front.DataGrid.CondDesign.Entities;

public partial class DataGridManageButtonsVm : ObservableObject, IDisposable
{
    private readonly IConditionalDesignationRepository _repository;
    private readonly CognDesignGridVm _condDesignGridVm;
    public DataGridManageButtonsVm(IConditionalDesignationRepository repository, CognDesignGridVm condDesignGridVm) {
        _repository = repository;
        _condDesignGridVm = condDesignGridVm;
        _condDesignGridVm.PropertyChanged += OnSelectedItemPropertyChanged;
    }
    public virtual void Dispose() {
        _condDesignGridVm.PropertyChanged -= OnSelectedItemPropertyChanged;
    }

    [RelayCommand]
    protected async Task AddItem() {
        var newItem = new ConditionalDesignation("Новое обозначение", "НО");
        await _repository.AddAsync(newItem);
        _condDesignGridVm.Items.Add(newItem);
        _condDesignGridVm.SelectedItem = newItem;
    }

    protected bool CanAddItem() {
        return true;
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
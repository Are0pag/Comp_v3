using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.TechnicalItems;
using Comp.Db.Contracts;
using RelayCommand = Utils.WPF.Mvvm.RelayCommand;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class CognDesignGridVm : ObservableObject
{
    private readonly IConditionalDesignationRepository _repository;
    private ConditionalDesignation _selectedItem;

    public ObservableCollection<ConditionalDesignation> Items { get; set; }

    public ConditionalDesignation SelectedItem {
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
            DeleteItemCommand.NotifyCanExecuteChanged();
        }
    }

    public CognDesignGridVm(IConditionalDesignationRepository repository) {
        _repository = repository;
        LoadDataAsync();
    }
    
    private async void LoadDataAsync() {
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<ConditionalDesignation>(items);
        OnPropertyChanged(nameof(Items));
    }

    [RelayCommand]
    private async Task AddItem() {
        var newItem = new ConditionalDesignation("Новое обозначение", "НО");
        await _repository.AddAsync(newItem);
        Items.Add(newItem);
        SelectedItem = newItem;
    }

    [RelayCommand(CanExecute = nameof(CanDeleteItem))]
    private async Task DeleteItemAsync() {
        if (SelectedItem == null) return;
        await _repository.DeleteAsync(SelectedItem.Id);
        Items.Remove(SelectedItem);
        SelectedItem = null;
    }

    private bool CanDeleteItem() {
        return SelectedItem != null;
    }
}
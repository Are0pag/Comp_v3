using System.Collections.ObjectModel;
using System.Windows.Input;
using Comp.ModelData.TechnicalItems;
using CL_CompDb;
using Utils.WPF.Mvvm;

namespace Comp_v3.Front.DataGrid.CondDesign;

public class MainVm : NotifyPropertyChanged
{
    private readonly IConditionalDesignationRepository _repository;
    private ConditionalDesignation _selectedItem;

    public ObservableCollection<ConditionalDesignation> Items { get; set; }

    public ConditionalDesignation SelectedItem {
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }

    public MainVm(IConditionalDesignationRepository repository) {
        _repository = repository;
        LoadDataAsync();

        AddCommand = new RelayCommand(AddItem);
        DeleteCommand = new RelayCommand(DeleteItem, CanDeleteItem);
        SaveCommand = new RelayCommand(SaveChanges);
    }
    
    private async void LoadDataAsync() {
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<ConditionalDesignation>(items);
        OnPropertyChanged(nameof(Items));
    }

    private async void AddItem(object? parameter) {
        var newItem = new ConditionalDesignation("Новое обозначение", "НО");
        await _repository.AddAsync(newItem);
        Items.Add(newItem);
        SelectedItem = newItem;
    }

    private async void DeleteItem(object? parameter) {
        if (SelectedItem == null) return;
        _repository.DeleteAsync(SelectedItem.Id);
        Items.Remove(SelectedItem);
        SelectedItem = null;
    }

    private bool CanDeleteItem(object? parameter) => SelectedItem != null;

    private async void SaveChanges(object? parameter) {
        if (SelectedItem != null) {
            _repository.UpdateAsync(SelectedItem);
        }
    }
}
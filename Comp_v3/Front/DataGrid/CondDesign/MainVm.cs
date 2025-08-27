using System.Collections.ObjectModel;
using System.Windows.Input;
using CL_Comp_ModelData.TechnicalItems;
using CL_CompDb;
using Utils.WPF.Mvvm;

namespace Comp_v3.Front.DataGrid.CondDesign;

public class MainVm : NotifyPropertyChanged
{
    private readonly ConditionalDesignationRepository _repository;
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

    public MainVm() {
        _repository = new ConditionalDesignationRepository();
        Items = new ObservableCollection<ConditionalDesignation>(_repository.GetAll());

        AddCommand = new RelayCommand(AddItem);
        DeleteCommand = new RelayCommand(DeleteItem, CanDeleteItem);
        SaveCommand = new RelayCommand(SaveChanges);
    }

    private void AddItem(object? parameter) {
        var newItem = new ConditionalDesignation("Новое обозначение", "НО");
        _repository.Add(newItem);
        Items.Add(newItem);
        SelectedItem = newItem;
    }

    private void DeleteItem(object? parameter) {
        if (SelectedItem == null) return;
        _repository.Delete(SelectedItem.Id);
        Items.Remove(SelectedItem);
        SelectedItem = null;
    }

    private bool CanDeleteItem(object? parameter) => SelectedItem != null;

    private void SaveChanges(object? parameter) {
        if (SelectedItem != null) {
            _repository.Update(SelectedItem);
        }
    }
}
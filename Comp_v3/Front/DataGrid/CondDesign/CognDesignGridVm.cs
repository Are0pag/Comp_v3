using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Comp.Db.Contracts;
using Component_v2.Tools.EventBus;
using RelayCommand = Utils.WPF.Mvvm.RelayCommand;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class CognDesignGridVm : ObservableObject, IDisposable, ICellEditEndingHandler
{
    private readonly IConditionalDesignationRepository _repository;
    private ConditionalDesignation _selectedItem;

    public CognDesignGridVm(IConditionalDesignationRepository repository) {
        _repository = repository;
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
        LoadDataAsync();
    }

    public void Dispose() {
        EventBus<IUiGlobalSubscriber>.Unsubscribe(this);
    }

    public ObservableCollection<ConditionalDesignation> Items { get; set; }

    public ConditionalDesignation SelectedItem {
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
            DeleteItemCommand.NotifyCanExecuteChanged();
        }
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

    async void ICellEditEndingHandler.HandleCellEdit(object? sender, DataGridCellEditEndingEventArgs e) {
        if (e.EditAction != DataGridEditAction.Commit) return;

        if (e.Row.Item is ConditionalDesignation editedItem) {
            await _repository.UpdateAsync(editedItem);
        }
    }
}
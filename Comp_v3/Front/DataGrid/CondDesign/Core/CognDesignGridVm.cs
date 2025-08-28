using System.Collections.ObjectModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Comp.Db.Contracts;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign;

public class CognDesignGridVm : ObservableObject, ICellEditEndingHandler
{
    private ConditionalDesignation _selectedItem;

    public CognDesignGridVm(IConditionalDesignationRepository repository, StateProviderDg stateProviderDg) {
        Repository = repository;
        StateProvider = stateProviderDg;
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
        LoadDataAsync();
    }

    public virtual void Dispose() => EventBus<IUiGlobalSubscriber>.Unsubscribe(this);

    public IConditionalDesignationRepository Repository { get; }
    public StateProviderDg StateProvider { get; }
    
    public ObservableCollection<ConditionalDesignation> Items { get; set; }
    public ConditionalDesignation SelectedItem {
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }
    private async void LoadDataAsync() {
        var items = await Repository.GetAllAsync();
        Items = new ObservableCollection<ConditionalDesignation>(items);
        OnPropertyChanged(nameof(Items));
    }
    async Task ICellEditEndingHandler.HandleCellEdit(object? sender, DataGridCellEditEndingEventArgs e) {
        await StateProvider.CurrentStateDataGrid.OnCellEditEnding(this, sender, e);
    }
}
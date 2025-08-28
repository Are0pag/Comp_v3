using System.Collections.ObjectModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Comp.Db.Contracts;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class CognDesignGridVm : ObservableObject, ICellEditEndingHandler
{
    private ConditionalDesignation _selectedItem;

    public CognDesignGridVm(IConditionalDesignationRepository repository) {
        Repository = repository;
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
        CurrentStateDataGrid = StateEditing;
        LoadDataAsync();
    }

    public virtual void Dispose() => EventBus<IUiGlobalSubscriber>.Unsubscribe(this);
    
    public StateDataGrid CurrentStateDataGrid { get; protected set; }
    public StateDgEditing StateEditing { get; protected set; } = new StateDgEditing();


    public IConditionalDesignationRepository Repository { get; }
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
        await CurrentStateDataGrid.OnCellEditEnding(this, sender, e);
    }
}
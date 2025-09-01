using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;
using Comp_v3.Front.Events;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Entities;

public class CognDesignGridVm : ObservableObject, ICellEditEndingHandler, ICellAddingToDataGridHandler, ICancelNewItemAddingHandler
{
    private ConditionalDesignation? _selectedItem; /* база 100% */

    public CognDesignGridVm(IConditionalDesignationRepository repository, StateProviderDg stateProviderDg) { /* Ui-взаимодействующий : VmRepo */
        Repository = repository;
        StateProvider = stateProviderDg;
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
        LoadDataAsync();
    }

    public virtual void Dispose() => EventBus<IUiGlobalSubscriber>.Unsubscribe(this);  /* Ui-взаимодействующий : VmRepo */

    public IConditionalDesignationRepository Repository { get; } /* VmRepo : базы */

    public StateProviderDg StateProvider { get; } /* Ui-взаимодействующий : VmRepo */

    public required ObservableCollection<ConditionalDesignation?> Items { get; set; } /* свойство должно быть обязательно инициализировано при создании объекта */ /* база 100% */

    public ConditionalDesignation? SelectedItem { /* база 100% */
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    private async void LoadDataAsync() {  /* VmRepo : базы */
        var items = await Repository.GetAllAsync();
        Debug.Assert(items != null, nameof(items) + " != null from IConditionalDesignationRepository");
        Items = new ObservableCollection<ConditionalDesignation?>(items!);
        OnPropertyChanged(nameof(Items));
    }

    void ICancelNewItemAddingHandler.HandleCancelNewItemAdding() {  /* Одна из шаблонных реализаций : Ui-взаимодействующий */
        if (StateProvider.CurrentStateDataGrid is not StateDgCreatingNewItem) return;
        if (SelectedItem == null) throw new Exception("Selected item is null");
        Items.Remove(SelectedItem);
        SelectedItem = null;
    }

    void ICellAddingToDataGridHandler.HandleNewValueAdding() { /* Одна из шаблонных реализаций : Ui-взаимодействующий */
        StateProvider.CurrentStateDataGrid.AddItemAsync(this);
    }

    async Task ICellEditEndingHandler.HandleCellEdit(object? sender, DataGridCellEditEndingEventArgs e) { /* Одна из шаблонных реализаций : Ui-взаимодействующий */
        await StateProvider.CurrentStateDataGrid.OnCellEditEnding(this, sender, e);
    }
}
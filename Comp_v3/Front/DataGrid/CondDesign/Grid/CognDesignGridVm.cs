using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v3.Front.DataGrid.CondDesign.Grid.States;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.ViewInvoking.GridItemsInteractions;
using Comp_v3.Front.Events.ViewInvoking.Keys;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid;

public class CognDesignGridVm : VmEnumerableInteractiveData<ConditionalDesignation>, 
                                ICellEditEndingHandler, ICellAddingToDataGridHandler, ICancelNewItemAddingHandler,
                                IPreviewKeyDownHandler
{
    public CognDesignGridVm(IConditionalDesignationRepository repository, StateProviderDg stateProviderDg) { /* Ui-взаимодействующий : VmRepo */
        Repository = repository;
        StateProvider = stateProviderDg;
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
        LoadDataAsync();
    }
    public virtual void Dispose() => EventBus<IUiGlobalSubscriber>.Unsubscribe(this);  /* Ui-взаимодействующий : VmRepo */

    public IConditionalDesignationRepository Repository { get; } /* VmRepo : базы */

    public StateProviderDg StateProvider { get; } /* Ui-взаимодействующий : VmRepo */

    private async void LoadDataAsync() {  /* VmRepo : базы */
        var items = await Repository.GetAllAsync();
        Debug.Assert(items != null, nameof(items) + " != null from IConditionalDesignationRepository");
        Items = new ObservableCollection<ConditionalDesignation?>(items!);
        OnPropertyChanged(nameof(Items));
    }

    void ICancelNewItemAddingHandler.HandleCancelNewItemAdding() {  /* Одна из шаблонных реализаций : Ui-взаимодействующий */
        if (StateProvider.CurrentState is not StateDgCreatingNewItem) return;
        if (SelectedItem == null) throw new Exception("Selected item is null");
        Items.Remove(SelectedItem);
        SelectedItem = null;
    }

    void ICellAddingToDataGridHandler.HandleNewValueAdding() { /* Одна из шаблонных реализаций : Ui-взаимодействующий */
        StateProvider.CurrentState.AddItemAsync(this);
    }

    async Task ICellEditEndingHandler.HandleCellEdit(object? sender, DataGridCellEditEndingEventArgs e) { /* Одна из шаблонных реализаций : Ui-взаимодействующий */
        await StateProvider.CurrentState.OnCellEditEnding(this, sender, e);
    }
    
    void IPreviewKeyDownHandler.HandleKeyInput(object? sender, KeyEventArgs e) {
        StateProvider.CurrentState.OnHandleKeyInput(this, sender, e);
    }
}
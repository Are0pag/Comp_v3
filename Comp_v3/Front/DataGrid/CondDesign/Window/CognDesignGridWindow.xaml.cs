using System.Windows.Controls;
using System.Windows.Input;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;
using Comp_v3.Front.DataGrid.CondDesign.Window.States;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.ViewInvoking.Keys;
using Comp_v3.Front.Events.VmInvoking.Request;
using Utils.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Window;

public partial class CognDesignGridWindow : System.Windows.Window, INewValueTryAddingToDataGridHandler, IDataGridRequestResolver
{
    public Provider StateProvider { get; }
    
    public CognDesignGridWindow(CognDesignGridVm cdDg, 
                                AddNewItemButVm aniCom, DeleteItemButVm diCom, SaveChangesButVm scCom,
                                Provider stateProvider) {
        InitializeComponent();
        SetDataContexts(cdDg, aniCom, diCom, scCom);
        StateProvider = stateProvider;
        EventBus<IVmGlobalSubscriber>.Subscribe(this);
    }

    public virtual void Dispose() {
        EventBus<IVmGlobalSubscriber>.Unsubscribe(this);
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        StateProvider.CurrentStateWindow.OnCellEditEnding(this, sender, e);
    }

    /* Суть в том, что надо при создании нового ряда в таблице (нового экземпляра модели соотв-но)
            автоматически прокрутить до новых ячеек и поставить курсор для редактирования на первую колонку (первую ячейку) */
    async Task INewValueTryAddingToDataGridHandler.HandleNewValueAdded(object? newValue) {
        StateProvider.SwitchStateWindow(StateProvider.StateCreatingNewItem, this);
        await StateProvider.CurrentStateWindow.OneNewValueAdded(this, newValue);
    }

    private void InfoDataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e) {
        StateProvider.CurrentStateWindow.OnBeginningEdit(this, sender, e);
    }

    private void SetDataContexts(CognDesignGridVm cdDg, AddNewItemButVm aniCom, DeleteItemButVm diCom, SaveChangesButVm scCom) {
        InfoDataGrid.DataContext = cdDg;
        AddNewItemButton.DataContext = aniCom;
        DeleteItemButton.DataContext = diCom;
        SaveChangesButton.DataContext = scCom;
        InfoDataGridContextMenuAddNewItemCommand.DataContext = aniCom;
        InfoDataGridContextMenuDeleteItemCommand.DataContext = diCom;
    }

    private void InfoDataGrid_OnPreviewKeyDown(object? sender, KeyEventArgs e) {
        EventBus<IUiGlobalSubscriber>.RaiseEvent<IPreviewKeyDownHandler>(h => h?.HandleKeyInput(sender, e));
    }
    
    void IDataGridRequestResolver.GetGrid(IDataGridRequester requester) => requester.DataGrid = InfoDataGrid;
}
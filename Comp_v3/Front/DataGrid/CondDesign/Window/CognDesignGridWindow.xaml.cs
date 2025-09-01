using System.Windows.Controls;
using System.Windows.Input;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Comp_v3.Front.DataGrid.CondDesign.Window.States;
using Comp_v3.Front.Events;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Window;

public partial class CognDesignGridWindow : System.Windows.Window, INewValueTryAddingToDataGridHandler
{
    public Provider StateProvider { get; }
    
    public CognDesignGridWindow(CognDesignGridVm cognDesignGridVm, DataGridManageButtonsVm dataGridManageButtonsVm, Provider stateProvider) {
        InitializeComponent();
        StateProvider = stateProvider;

        InfoDataGrid.DataContext = cognDesignGridVm;
        AddNewItemButton.DataContext = dataGridManageButtonsVm;
        DeleteItemButton.DataContext = dataGridManageButtonsVm;
        EventBus<IVmGlobalSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IVmGlobalSubscriber>.Unsubscribe(this);
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        StateProvider.CurrentStateWindow.OnCellEditEnding(this, sender, e);
    }

    /* Суть в том, что надо при создании нового ряда в таблице (нового экземпляра модели соотв-но)
            автоматически прокрутить до новых ячеек и поставить курсор для редактирования на первую колонку (первую ячейку) */
    void INewValueTryAddingToDataGridHandler.HandleNewValueAdded(object newValue) {
        StateProvider.SwitchStateWindow(StateProvider.StateCreatingNewItem, this);
        StateProvider.CurrentStateWindow.OneNewValueAdded(this, newValue);
    }

    private void InfoDataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e) {
        StateProvider.CurrentStateWindow.OnBeginningEdit(this, sender, e);
    }
}
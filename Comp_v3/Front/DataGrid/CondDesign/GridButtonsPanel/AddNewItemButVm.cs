using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.ViewInvoking.GridItemsInteractions;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;

public partial class AddNewItemButVm : BaseButtonsVm, ICellAddingToDataGridHandler, ICancelNewItemAddingHandler
{
    public AddNewItemButVm(CognDesignGridVm condDesignGridVm) : base(condDesignGridVm) {
        EventBus<IUiGlobalSubscriber>.Subscribe(this);
    }

    public override void Dispose() {
        base.Dispose();
        EventBus<IUiGlobalSubscriber>.Unsubscribe(this);
    }

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    protected async Task AddItem() {
        await _condDesignGridVm.StateProvider.CurrentState.AddItemAsync(_condDesignGridVm);
    }

    protected bool CanAddItem() {
        return _condDesignGridVm.StateProvider.CurrentState.CanAddItem(_condDesignGridVm);
    }
    
    void ICancelNewItemAddingHandler.HandleCancelNewItemAdding() {
        AddItemCommand.NotifyCanExecuteChanged();
    }

    void ICellAddingToDataGridHandler.HandleNewValueAdding() {
        AddItemCommand.NotifyCanExecuteChanged();
    }
}
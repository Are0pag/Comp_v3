using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.Buttons;
using Comp_v3.Front.Events.ViewInvoking.GridItemsInteractions;
using Comp_v3.Front.Events.ViewInvoking.Keys;
using Utils.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;

public partial class AddNewItemButVm : BaseButtonsVm, ICellAddingToDataGridHandler, ICancelNewItemAddingHandler, IPreviewKeyDownHandler
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

    public override void NotifyCanExecute() {
        AddItemCommand.NotifyCanExecuteChanged();
    }

    async Task IPreviewKeyDownHandler.HandleKeyInput(object? sender, KeyEventArgs e) {
        AddItemCommand.NotifyCanExecuteChanged();
    }

    void ICancelNewItemAddingHandler.HandleCancelNewItemAdding() {
        AddItemCommand.NotifyCanExecuteChanged();
    }

    void ICellAddingToDataGridHandler.HandleNewValueAdding() {
        AddItemCommand.NotifyCanExecuteChanged();
    }
}
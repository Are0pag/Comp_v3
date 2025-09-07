using Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;
using Comp_v3.Front.DataGrid.CondDesign.Transactions;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Utils.EventBus;
using WPF.Services.UserActionsHandling.InputKey;
using Commands = Comp_v3.Front.DataGrid.CondDesign.Commands;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgCreatingNewItem : StateDataGrid
{
    public StateDgCreatingNewItem(HeterochromicCommandScheduler scheduler, CommonUndoRedoHotKeysService commonKeysService) 
        : base(scheduler, commonKeysService) {
    }

    public ConditionalDesignation? CreatingItem { get; protected set; }

    public override async Task Enter(CognDesignGridVm vm) {
        var cmd = new PreparerNewRawCommand(vm);
        await _scheduler.RegisterCommandInto<CreateNewRawAndFocusTransaction>(cmd)
                        .ExecuteLastRegisteredAsync();
        
        EventBus<IVmGlobalSubscriber>.RaiseEvent<INewValueTryAddingToDataGridHandler>(h => h?.HandleNewValueAdded(cmd.CreatingItem!));
        CreatingItem = cmd.CreatingItem;
    }

    public override async Task AddItemAsync(CognDesignGridVm vm) {
        
        await _scheduler.RegisterCommandInto<AddingNewItemTransaction>(new PreparerEditingRawCommand(null))
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<AddingNewItemTransaction>(new AddItemCommand(vm, CreatingItem!))
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<AddingNewItemTransaction>(new Commands.ChangeTargetVmStateCommand(vm, this, vm.StateProvider.GetState<StateDgEditing>()))
                        .ExecuteLastRegisteredAsync();
    }
}
using System.Diagnostics;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;
using Infrastructure.Command.Heterochromic;
using WPF.Services.UserActionsHandling.InputKey;
using Commands = Comp_v3.Front.DataGrid.CondDesign.Commands;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgCreatingNewItem : StateDataGrid
{
    public StateDgCreatingNewItem(HeterochromicCommandScheduler<IDeferredCommand> scheduler, CommonUndoRedoHotKeysService commonKeysService) 
        : base(scheduler, commonKeysService) {
    }

    public ConditionalDesignation? CreatingConditionalDesignation { get; protected set; }

    public override async Task Enter(CognDesignGridVm vm) {
        _scheduler.BeginTransaction();
        var cmd = new Commands.AddingNewItem.PreparerCommand(vm);
        await _scheduler.ExecuteCommand(cmd);
        EventBus<IVmGlobalSubscriber>.RaiseEvent<INewValueTryAddingToDataGridHandler>(h => h.HandleNewValueAdded(cmd.CreatingItem!));
    }

    public override async Task AddItemAsync(CognDesignGridVm vm) {
        Debug.Assert(CreatingConditionalDesignation != null, nameof(CreatingConditionalDesignation) + " != null");
        await vm.Repository.AddAsync(CreatingConditionalDesignation); /* Model Command */
        await vm.StateProvider.ChangeState(vm.StateProvider.GetState<StateDgEditing>(), vm);
    }
}
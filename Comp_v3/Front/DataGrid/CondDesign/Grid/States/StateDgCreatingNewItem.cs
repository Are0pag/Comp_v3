using System.Diagnostics;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;
using Infrastructure.Command.Classic;

namespace Comp_v3.Front.DataGrid.CondDesign.Grid.States;

public class StateDgCreatingNewItem : StateDataGrid
{
    public StateDgCreatingNewItem(HeterochromicCommandScheduler<IDeferredCommand> scheduler) : base(scheduler) {
    }

    public ConditionalDesignation? CreatingConditionalDesignation { get; protected set; }

    public override Task Enter(CognDesignGridVm vm) {
        CreatingConditionalDesignation = new ConditionalDesignation("", "");
        vm.Items.Add(CreatingConditionalDesignation);
        vm.SelectedItem = CreatingConditionalDesignation;
        
        EventBus<IVmGlobalSubscriber>.RaiseEvent<INewValueTryAddingToDataGridHandler>(h => h.HandleNewValueAdded(CreatingConditionalDesignation));
        return Task.CompletedTask;
    }

    public override async Task AddItemAsync(CognDesignGridVm vm) {
        Debug.Assert(CreatingConditionalDesignation != null, nameof(CreatingConditionalDesignation) + " != null");
        await vm.Repository.AddAsync(CreatingConditionalDesignation); /* Model Command */
        await vm.StateProvider.ChangeState(vm.StateProvider.GetState<StateDgEditing>(), vm);
    }
}
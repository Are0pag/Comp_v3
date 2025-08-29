using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;

public class StateDgCreatingNewItem : StateDataGrid
{
    public ConditionalDesignation CreatingConditionalDesignation { get; protected set; }

    public override void Entry(CognDesignGridVm vm) {
        CreatingConditionalDesignation = new ConditionalDesignation("", "");
        vm.Items.Add(CreatingConditionalDesignation);
        vm.SelectedItem = CreatingConditionalDesignation;
        
        EventBus<IVmGlobalSubscriber>.RaiseEvent<INewValueTryAddingToDataGridHandler>(h => h.HandleNewValueAdded(CreatingConditionalDesignation));
    }

    public override async Task AddItemAsync(CognDesignGridVm vm) {
        await vm.Repository.AddAsync(CreatingConditionalDesignation);
        vm.StateProvider.ChangeState(vm.StateProvider.Editing);
    }
}
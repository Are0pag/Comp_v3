using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Window;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Utils.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class CancelEditCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>, IDataGridRequester<CognDesignGridWindow>
{
    public CancelEditCommand(CognDesignGridVm context) : base(context) {
    }

    public System.Windows.Controls.DataGrid DataGrid { get; set; }

    public override Task ExecuteAsync() {
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IDataGridRequestResolver<CognDesignGridWindow>>(r => r.GetGrid(this));
        DataGrid!.CancelEdit();
        return Task.CompletedTask;
    }

    public override async Task UndoAsync() {
        
    }

    public override async Task ExecuteDeferredAsync() {
        
    }
}
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class AddItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public AddItemCommand(CognDesignGridVm context) : base(context) {
    }

    public override async Task ExecuteAsync() {
        throw new NotImplementedException();
    }

    public override async Task UndoAsync() {
        throw new NotImplementedException();
    }

    public override async Task ExecuteDeferredAsync() {
        throw new NotImplementedException();
    }
}
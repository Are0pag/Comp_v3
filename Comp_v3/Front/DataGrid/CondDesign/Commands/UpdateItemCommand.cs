using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class UpdateItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public UpdateItemCommand(CognDesignGridVm context, ConditionalDesignation item) : base(context) {
        _item = item;
    }

    public override Task ExecuteAsync() {
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        return Task.CompletedTask;
    }

    public override async Task ExecuteDeferredAsync() {
        await _context.Repository.UpdateAsync(_item);
    }
}
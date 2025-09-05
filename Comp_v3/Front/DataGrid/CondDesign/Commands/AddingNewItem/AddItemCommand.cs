using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Window;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;

public class AddItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public AddItemCommand(CognDesignGridVm context, ConditionalDesignation item) : base(context) {
        _item = item;
    }

    public override Task ExecuteAsync() {
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        return Task.CompletedTask;
    }

    public override async Task ExecuteDeferredAsync() {
        await _context.Repository.AddAsync(_item);
    }
}
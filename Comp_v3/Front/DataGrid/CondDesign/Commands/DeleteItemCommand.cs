using System.Diagnostics;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp.ModelData.TechnicalItems;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class DeleteItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public DeleteItemCommand(CognDesignGridVm context) : base(context) {
    }

    public override Task ExecuteAsync() {
        _item = _context.SelectedItem;
        _context.Items.Remove(_context.SelectedItem);
        _context.SelectedItem = null;

        return Task.CompletedTask;
    }

    public async Task UndoAsync() {
        throw new NotImplementedException();
    }

    public override async Task ExecuteDeferredAsync() {
        await _context.Repository.DeleteAsync(_item.Id);
        _item = null;
    }

    public virtual string Description { get; set;} = $"DeleteCommand";
}
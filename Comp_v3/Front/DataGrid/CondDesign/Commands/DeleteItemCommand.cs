using System.Diagnostics;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Classic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class DeleteItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public DeleteItemCommand(CognDesignGridVm context) : base(context) {
    }

    public override Task ExecuteAsync() {
        Debug.Assert(_context.SelectedItem == null, "_context.SelectedItem != null");
        _context.Items.Remove(_context.SelectedItem);
        _item = _context.SelectedItem;
        _context.SelectedItem = null;

        return Task.CompletedTask;
    }

    public async Task UndoAsync() {
        throw new NotImplementedException();
    }

    public override async Task ExecuteDeferredAsync() {
        Debug.Assert(_item == null, "_context.SelectedItem != null");
        await _context.Repository.DeleteAsync(_item.Id);
        _item = null;
    }
}

public class DeferredCommand<TContext, TItem> : IDeferredCommand
{
    protected readonly TContext _context;
    protected TItem? _item;

    public DeferredCommand(TContext context) {
        _context = context;
    }

    public async virtual Task ExecuteAsync() {
        throw new NotImplementedException();
    }

    public async virtual Task UndoAsync() {
        throw new NotImplementedException();
    }

    public async virtual Task ExecuteDeferredAsync() {
        throw new NotImplementedException();
    }
}
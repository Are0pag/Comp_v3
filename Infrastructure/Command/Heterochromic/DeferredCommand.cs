using Infrastructure.Command.Classic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

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
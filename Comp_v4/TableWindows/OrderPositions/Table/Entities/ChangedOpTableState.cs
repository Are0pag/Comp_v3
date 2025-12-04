using Comp.ModelData;

namespace Comp_v4.TableWindows.OrderPositions.Table.Entities;

public class ChangedOpTableState : BaseOpState
{
    public override async Task Create(TaskCompletionSource tcs, OpTable opTable, object? o) {
        throw new NotImplementedException();
    }

    public override async Task Edit(TaskCompletionSource tcs, OpTable opTable, OrderPosition op, object? o) {
        throw new NotImplementedException();
    }
}
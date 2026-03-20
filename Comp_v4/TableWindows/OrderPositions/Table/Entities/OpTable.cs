using Comp.ModelData;
using Infrastructure.StateMachine;

namespace Comp_v4.TableWindows.OrderPositions.Table.Entities;

public class OpTable : GenericStateMachine<BaseOpState, OpTable>
{
    public OpTable(IEnumerable<BaseOpState> states, BaseOpState initialState) : base(states, initialState) {
        //Console.WriteLine("f");
    }

    public virtual async Task Create(TaskCompletionSource tcs, object? arg = null) {
        await CurrentState.Create(tcs, this, arg);
    }

    public virtual async Task Edit(TaskCompletionSource tcs, OrderPosition op, object? arg = null) {
        await CurrentState.Edit(tcs, this, op, arg);
    }
}

public abstract class BaseOpState : StateBase<OpTable>
{
    public abstract Task Create(TaskCompletionSource tcs, OpTable opTable, object? o);
    public abstract Task Edit(TaskCompletionSource tcs, OpTable opTable, OrderPosition op, object? o);
}
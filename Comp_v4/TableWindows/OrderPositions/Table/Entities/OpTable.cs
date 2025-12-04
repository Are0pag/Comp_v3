using Infrastructure.StateMachine;

namespace Comp_v4.TableWindows.OrderPositions.Table.Entities;

public class OpTable : GenericStateMachine<BaseOpState, OpTable>
{
    protected OpTable(IEnumerable<BaseOpState> states, BaseOpState initialState) : base(states, initialState) {
    }
    
    
}

public abstract class BaseOpState : StateBase<OpTable>
{
    
}
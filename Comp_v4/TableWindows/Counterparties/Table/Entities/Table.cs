using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table._Installers;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Table.Entities;

public class Table : GenericStateMachine<BaseTableState, Table>
{
    public Table(IEnumerable<BaseTableState> states, BaseTableState initialState) : base(states, initialState) {
    }

    public void Dispose() {
    }
}

public abstract class BaseTableState : StateBase<Table>
{
}

public class EditTableState : BaseTableState
{
    
}
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Actions;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Table.Entities;

public class TableCounterparty : GenericStateMachine<BaseCpTableState, TableCounterparty>, IMouseDoubleClickHandler
{
    protected readonly CounterpartyTableWindow _counterpartyTableWindow;
    public TableCounterparty(IEnumerable<BaseCpTableState> states, BaseCpTableState initialState, CounterpartyTableWindow counterpartyTableWindow) : base(states, initialState) {
        _counterpartyTableWindow = counterpartyTableWindow;
        _counterpartyTableWindow.OnDoubleClickSelectingItemInTable += (tcs, sender, args) => {
            _ = OnMouseDoubleClick(tcs, sender, args);
        };
    }
    public void Dispose() {
    }

    public async Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs e) {
        await CurrentState.OnMouseDoubleClick(this, sender, e, tcs);
    }
}

public abstract class BaseCpTableState : StateBase<TableCounterparty>
{
    public abstract Task OnMouseDoubleClick(TableCounterparty table, object sender, MouseButtonEventArgs mouseButtonEventArgs, TaskCompletionSource tcs);
}

public class EditCpTableState : BaseCpTableState
{
    protected readonly EditCounterpartyAction _editCounterpartyAction;
    public EditCpTableState(EditCounterpartyAction editCounterpartyAction) {
        _editCounterpartyAction = editCounterpartyAction;
    }

    public override async Task OnMouseDoubleClick(TableCounterparty table, object sender, MouseButtonEventArgs mouseButtonEventArgs, TaskCompletionSource tcs) {
        if (sender is not DataGrid { SelectedItem: Counterparty })
            throw new Exception();

        if (_editCounterpartyAction.CanPerform()) {
            await _editCounterpartyAction.Perform(tcs);
        }
            
    }
}
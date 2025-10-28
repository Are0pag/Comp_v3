using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Table.Entities;

public class TableCounterparty : GenericStateMachine<BaseCpTableState, TableCounterparty>, IMouseDoubleClickHandler
{
    public TableCounterparty(IEnumerable<BaseCpTableState> states, BaseCpTableState initialState) : base(states, initialState) {
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
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
    protected readonly ICounterpartyFormHandler _formContextInstaller;

    public EditCpTableState(ICounterpartyFormHandler formContextInstaller) {
        _formContextInstaller = formContextInstaller;
    }

    public override async Task OnMouseDoubleClick(TableCounterparty table, object sender, MouseButtonEventArgs mouseButtonEventArgs, TaskCompletionSource tcs) {
        if (sender is not DataGrid { SelectedItem: Counterparty counterparty })
            throw new Exception();
            
        await _formContextInstaller.Open<EditCpFormState>(tcs, counterparty);
    }
}
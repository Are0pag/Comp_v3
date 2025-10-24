using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table._Installers;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Table.Entities;

public class Table : GenericStateMachine<BaseTableState, Table>, IMouseDoubleClickHandler
{
    public Table(IEnumerable<BaseTableState> states, BaseTableState initialState) : base(states, initialState) {
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public async Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs e) {
        await CurrentState.OnMouseDoubleClick(this, sender, e, tcs);
    }
}

public abstract class BaseTableState : StateBase<Table>
{
    public abstract Task OnMouseDoubleClick(Table table, object sender, MouseButtonEventArgs mouseButtonEventArgs, TaskCompletionSource tcs);
}

public class EditTableState : BaseTableState
{
    protected readonly FormContextInstaller _formContextInstaller;

    public EditTableState(FormContextInstaller formContextInstaller) {
        _formContextInstaller = formContextInstaller;
    }

    public override async Task OnMouseDoubleClick(Table table, object sender, MouseButtonEventArgs mouseButtonEventArgs, TaskCompletionSource tcs) {
        if (sender is not DataGrid { SelectedItem: Counterparty counterparty })
            throw new Exception();
            
        await _formContextInstaller.Open<EditFormState>(tcs, counterparty);
    }
}
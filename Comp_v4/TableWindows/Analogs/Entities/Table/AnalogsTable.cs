using System.Windows.Input;
using Comp_v4.TableWindows.Analogs.Events;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class AnalogsTable : GenericStateMachine<BaseAnalogsTableState, AnalogsTable>
{
    public AnalogsTable(IEnumerable<BaseAnalogsTableState> states, BaseAnalogsTableState initialState) : base(states, initialState) {
        //EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        //EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public async Task OnMouseDoubleClick(object sender, MouseButtonEventArgs e, TaskCompletionSource tcs) {
        await CurrentState.OnMouseDoubleClick(tcs, this, sender, e);
    }

    public async Task Add(TaskCompletionSource tcs) {
        await CurrentState.Add(tcs, this);
    }

    public async Task Edit(TaskCompletionSource tcs) {
        await CurrentState.Edit(tcs, this);
    }
}

public abstract class BaseAnalogsTableState : StateBase<AnalogsTable>
{
    public abstract Task OnMouseDoubleClick(TaskCompletionSource tcs, AnalogsTable table, object sender, MouseButtonEventArgs e);
    public abstract Task Add(TaskCompletionSource tcs, AnalogsTable analogsTable);
    public abstract Task Edit(TaskCompletionSource tcs, AnalogsTable analogsTable);
}

public class IdleAnalogTableState : BaseAnalogsTableState
{
    public override async Task OnMouseDoubleClick(TaskCompletionSource tcs, AnalogsTable table, object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }

    public override async Task Add(TaskCompletionSource tcs, AnalogsTable analogsTable) {
        throw new NotImplementedException();
    }

    public override async Task Edit(TaskCompletionSource tcs, AnalogsTable analogsTable) {
        throw new NotImplementedException();
    }
}
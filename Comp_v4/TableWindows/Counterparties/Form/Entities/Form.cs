using Comp_v4.TableWindows.Counterparties.Events;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Form.Entities;

public class Form : GenericStateMachine<BaseFormState, Form>, ISaveHandler
{
    public Form(IEnumerable<BaseFormState> states, BaseFormState initialState) : base(states, initialState) {
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public async Task Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null) {
        if (parameter is not Counterparty counterparty)
            throw new InvalidOperationException();
        await CurrentState.Save(this, parameter);
        tcs.SetResult(counterparty);
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    public abstract Task Save(Form form, object? parameter);
}

public class EditFormState : BaseFormState
{
    public override async Task Save(Form form, object? parameter) {
        throw new NotImplementedException();
    }
}

public class CreateFormState : BaseFormState
{
    public override async Task Save(Form form, object? parameter) {
        throw new NotImplementedException();
    }
}
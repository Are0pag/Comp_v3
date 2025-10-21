using Comp_v4.TableWindows.Counterparties.Form.Events;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Form.Entities;

public class Form : GenericStateMachine<BaseFormState, Form>, ISaveHandler
{
    protected Form(IEnumerable<BaseFormState> states, BaseFormState initialState) : base(states, initialState) {
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public void Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null) {
        CurrentState.Save(this, parameter);
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    public abstract void Save(Form form, object? parameter);
}

public class EditFormState : BaseFormState
{
    public override void Save(Form form, object? parameter) {
        throw new NotImplementedException();
    }
}

public class CreateFormState : BaseFormState
{
    public override void Save(Form form, object? parameter) {
        throw new NotImplementedException();
    }
}
using Comp_v4.TableWindows.Analogs.Events;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class Form : GenericStateMachine<BaseFormState, Form>, ISelectAnalogHandler
{
    public Form(IEnumerable<BaseFormState> states, BaseFormState initialState) : base(states, initialState) {
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public void OnStartSelectingAnalog(object? parameter = null) {
        CurrentState.OnStartSelectingAnalog(parameter);
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    public abstract void OnStartSelectingAnalog(object? parameter = null);
}

public class AddFormState : BaseFormState
{
    public override void OnStartSelectingAnalog(object? parameter = null) {
        
    }
}
public class EditFormState : BaseFormState
{
    public override void OnStartSelectingAnalog(object? parameter = null) {
        throw new NotImplementedException();
    }
}
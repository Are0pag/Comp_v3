using Comp_v4.NomDict.Events;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData.Comp;
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

    public async Task OnStartSelectingAnalog(object? parameter = null) {
        await CurrentState.OnStartSelectingAnalog(parameter);
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    public abstract Task OnStartSelectingAnalog(object? parameter = null);
}

public class AddFormState : BaseFormState
{
    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        if (parameter is not TaskCompletionSource<Component> completionSource)
            throw new ArgumentException("parameter must be a TaskCompletionSource");
        
        EventBus<INomDictWindowSubscriber>.RaiseEvent<IGridSelectingStateHandler>(h => h?.OnSelecting(completionSource));
        Component selectedItem = await completionSource.Task;
    }
}


public class EditFormState : BaseFormState
{
    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        throw new NotImplementedException();
    }
}

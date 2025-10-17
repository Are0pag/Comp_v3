using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.View;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Infrastructure.StateMachine;
using Utils.EventBus;
using Utils.WPF;

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
    protected readonly Analog _analog;
    protected readonly IWindowOrderLocator _windowOrderLocator;

    public AddFormState(Analog analog, IWindowOrderLocator windowOrderLocator) {
        _analog = analog;
        _windowOrderLocator = windowOrderLocator;
    }

    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        if (parameter is not TaskCompletionSource<Component> completionSource)
            throw new ArgumentException("parameter must be a TaskCompletionSource");
        
        EventBus<INomDictWindowSubscriber>.RaiseEvent<IGridSelectingStateHandler>(h => h?.OnSelecting(completionSource));
        _analog.RelatedComponent = await completionSource.Task;
        _windowOrderLocator.MoveToBack<NomDictWindow>();
    }
}


public class EditFormState : BaseFormState
{
    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        throw new NotImplementedException();
    }
}

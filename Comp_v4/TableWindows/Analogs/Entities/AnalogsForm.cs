using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class AnalogsForm : GenericStateMachine<BaseAnalogsFormState, AnalogsForm>, ISelectAnalogHandler, ISaveHandler
{
    public AnalogsForm(IEnumerable<BaseAnalogsFormState> states, BaseAnalogsFormState initialState) : base(states, initialState) {
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public async Task Save(TaskCompletionSource tcs, Analog analog) {
        await CurrentState.Save(this);
        tcs.SetResult();
    }

    public async Task OnStartSelectingAnalog(object? parameter = null) {
        await CurrentState.OnStartSelectingAnalog(parameter);
    }
}

public abstract class BaseAnalogsFormState : StateBase<AnalogsForm>
{
    public abstract Task OnStartSelectingAnalog(object? parameter = null);

    public abstract Task Save(AnalogsForm form);
}

public class EditAnalogsFormState : BaseAnalogsFormState
{
    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        throw new NotImplementedException();
    }

    public override async Task Save(AnalogsForm form) {
        throw new NotImplementedException();
    }
}

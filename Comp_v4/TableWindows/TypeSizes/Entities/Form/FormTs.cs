using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Events;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.TypeSizes.Entities.Form;

public class FormTs : GenericStateMachine<BaseTsStateForm, FormTs>, ITypeSizeCreateHandler
{
    public FormTs(IEnumerable<BaseTsStateForm> states, BaseTsStateForm initialState) : base(states, initialState) {
        EventBus<ITypeSizesWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ITypeSizesWindowSubscriber>.Unsubscribe(this);
    }

    public async Task OnCreate(object? parameter = null) {
        CurrentState?.OnCreate(parameter);
    }
}
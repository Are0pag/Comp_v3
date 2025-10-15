using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Events;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.TypeSizes.Entities.Form;

public class Form : GenericStateMachine<BaseStateForm, Form>, ITypeSizeCreateHandler
{
    public Form(IEnumerable<BaseStateForm> states, BaseStateForm initialState) : base(states, initialState) {
        EventBus<ITypeSizesWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ITypeSizesWindowSubscriber>.Unsubscribe(this);
    }

    public async Task OnCreate(object? parameter = null) {
        CurrentState?.OnCreate();
    }
}
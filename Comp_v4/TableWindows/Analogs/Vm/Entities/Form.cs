using Infrastructure.StateMachine;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class Form : GenericStateMachine<BaseFormState, Form>
{
    public Form(IEnumerable<BaseFormState> states, BaseFormState initialState) : base(states, initialState) {
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    
}

public class AddFormState : BaseFormState {}
public class EditFormState : BaseFormState {}
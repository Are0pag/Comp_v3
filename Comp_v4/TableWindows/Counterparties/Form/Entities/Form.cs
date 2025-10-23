using Comp_v4.TableWindows.Counterparties.Events;
using Comp.Db.Contracts;
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
        await CurrentState.Save(this, counterparty, parameter);
        tcs.SetResult(counterparty);
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    protected readonly IRepository<Counterparty> _repository;
    
    protected BaseFormState(IRepository<Counterparty> repository) {
        _repository = repository;
    }

    public abstract Task Save(Form form, Counterparty counterparty, object? parameter);
}

public class EditFormState : BaseFormState
{
    public EditFormState(IRepository<Counterparty> repository) : base(repository) {
    }

    public override async Task Save(Form form, Counterparty counterparty, object? parameter) {
        await _repository.UpdateAsync(counterparty);
    }
}

public class CreateFormState : BaseFormState
{
    public CreateFormState(IRepository<Counterparty> repository) : base(repository) {
    }

    public override async Task Save(Form form, Counterparty counterparty, object? parameter) {
        await _repository.AddAsync(counterparty);
    }
}
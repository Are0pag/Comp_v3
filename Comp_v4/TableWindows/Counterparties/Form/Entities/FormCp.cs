using Comp_v4.TableWindows.Counterparties.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Form.Entities;

public class FormCp : GenericStateMachine<BaseCpFormState, FormCp>, ISaveHandler
{
    public FormCp(IEnumerable<BaseCpFormState> states, BaseCpFormState initialState) : base(states, initialState) {
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

public abstract class BaseCpFormState : StateBase<FormCp>
{
    protected readonly IRepository<Counterparty> _repository;
    
    protected BaseCpFormState(IRepository<Counterparty> repository) {
        _repository = repository;
    }

    public abstract Task Save(FormCp form, Counterparty counterparty, object? parameter);
}

public class EditCpFormState : BaseCpFormState
{
    public EditCpFormState(IRepository<Counterparty> repository) : base(repository) {
    }

    public override async Task Save(FormCp form, Counterparty counterparty, object? parameter) {
        await _repository.UpdateAsync(counterparty);
    }
}

public class CreateCpFormState : BaseCpFormState
{
    public CreateCpFormState(IRepository<Counterparty> repository) : base(repository) {
    }

    public override async Task Save(FormCp form, Counterparty counterparty, object? parameter) {
        await _repository.AddAsync(counterparty);
    }
}
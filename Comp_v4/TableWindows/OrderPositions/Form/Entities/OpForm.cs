using Comp_v4.TableWindows.OrderPositions.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.OrderPositions.Form.Entities;

public class OpForm : GenericStateMachine<BaseOpFormState, OpForm>
{
    public OpForm(IEnumerable<BaseOpFormState> states, BaseOpFormState initialState) : base(states, initialState) {
    }

    public virtual async Task Save(TaskCompletionSource tcs, OrderPosition item, object? args = null) {
        await CurrentState.Save(tcs, item, args, this);
    }
}

public abstract class BaseOpFormState : StateBase<OpForm>
{
    protected readonly IRepository<OrderPosition> _repository;

    protected BaseOpFormState(IRepository<OrderPosition> repository) {
        _repository = repository;
    }

    public abstract Task Save(TaskCompletionSource tcs, OrderPosition item, object? args, OpForm opForm);
}

public class CreateOpFormState : BaseOpFormState
{
    public CreateOpFormState(IRepository<OrderPosition> repository) : base(repository) {
    }

    public override async Task Save(TaskCompletionSource tcs, OrderPosition item, object? args, OpForm opForm) {
        try {
            await _repository.AddAsync(item);
        }
        catch (Exception ex) {
            throw ex;
        }

        var savingTcs = new TaskCompletionSource();
        EventBus<IOrderPositionSubscriber>
           .RaiseEvent<IOrderPosSavingCommitHandler>(h => h?.OnSaveOp(savingTcs));
        await savingTcs.Task;
        
        tcs.TrySetResult();
    }
}

public class EditOpFormState : BaseOpFormState
{
    public EditOpFormState(IRepository<OrderPosition> repository) : base(repository) {
    }

    public override async Task Save(TaskCompletionSource tcs, OrderPosition item, object? args, OpForm opForm) {
        try {
            await _repository.UpdateAsync(item);
        }
        catch (Exception ex) {
            throw ex;
        }

        tcs.TrySetResult();
    }
}
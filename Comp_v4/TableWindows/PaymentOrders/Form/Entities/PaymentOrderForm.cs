using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;

namespace Comp_v4.TableWindows.PaymentOrders.Form.Entities;

public class PaymentOrderForm : GenericStateMachine<PaymentOrderFormBaseState, PaymentOrderForm>
{
    public PaymentOrderForm(IEnumerable<PaymentOrderFormBaseState> states, PaymentOrderFormBaseState initialState) : base(states, initialState) {
    }

    public async Task Save(TaskCompletionSource tcs, PaymentOrder item, object? parameter = null) {
        await CurrentState.Save(this, tcs, item, parameter);
    }
}

public abstract class PaymentOrderFormBaseState : StateBase<PaymentOrderForm>
{
    protected readonly IRepository<PaymentOrder> _repository;

    protected PaymentOrderFormBaseState(IRepository<PaymentOrder> repository) {
        _repository = repository;
    }

    public abstract Task Save(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter);
}

public class CreatePoState : PaymentOrderFormBaseState
{
    public CreatePoState(IRepository<PaymentOrder> repository) : base(repository) {
    }

    public override async Task Save(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter) {
        await _repository.AddAsync(item);
        tcs.TrySetResult();
    }
}

public class EditPoState : PaymentOrderFormBaseState
{
    public EditPoState(IRepository<PaymentOrder> repository) : base(repository) {
    }

    public override async Task Save(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter) {
        await _repository.UpdateAsync(item);
        tcs.TrySetResult();
    }
}
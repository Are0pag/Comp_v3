using Comp_v4.TableWindows.PaymentOrders.events;
using Comp_v4.TableWindows.PaymentOrders.Table;
using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.PaymentOrders.Form.Entities;

public class PaymentOrderForm : GenericStateMachine<PaymentOrderFormBaseState, PaymentOrderForm>
{
    public PaymentOrderForm(IEnumerable<PaymentOrderFormBaseState> states, PaymentOrderFormBaseState initialState) : base(states, initialState) {
    }

    public async Task Save(TaskCompletionSource tcs, PaymentOrder item, object? parameter = null) {
        await CurrentState.Save(this, tcs, item, parameter);
    }

    public async Task Cancel(TaskCompletionSource tcs, PaymentOrder item, object? parameter = null) {
        await CurrentState.Cancel(this, tcs, item, parameter);
    }
}

public abstract class PaymentOrderFormBaseState : StateBase<PaymentOrderForm>
{
    protected readonly IRepository<PaymentOrder> _repository;

    protected PaymentOrderFormBaseState(IRepository<PaymentOrder> repository) {
        _repository = repository;
    }

    public abstract Task Save(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter);
    public abstract Task Cancel(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter);
}

public class CreatePoState : PaymentOrderFormBaseState
{
    public CreatePoState(IRepository<PaymentOrder> repository) : base(repository) {
    }

    public override async Task Save(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter) {
        await _repository.AddAsync(item);
        tcs.TrySetResult();
    }

    public override async Task Cancel(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter) {
        new InstanceContainer<PaymentOrderFormWindow>().RuntimeParam.Close();
        tcs.TrySetResult();
    }
}

public class EditPoState : PaymentOrderFormBaseState, IStartEditingPo
{
    protected PaymentOrder _origin;
    public EditPoState(IRepository<PaymentOrder> repository) : base(repository) {
        EventBus<IPoSubscriber>.Subscribe(this);
    }

    public override async Task Save(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter) {
        await _repository.UpdateAsync(item);
        tcs.TrySetResult();
    }

    public override async Task Cancel(PaymentOrderForm paymentOrderForm, TaskCompletionSource tcs, PaymentOrder item, object? parameter) {
        item.PopulateFrom(_origin); // отмена изменений
        new InstanceContainer<PaymentOrderFormWindow>().RuntimeParam.Close();
        tcs.TrySetResult();
    }

    public void Dispose() {
        EventBus<IPoSubscriber>.Unsubscribe(this);
    }

    public async Task OnStartEditingPo(PaymentOrder paymentOrder) {
        _origin = new PaymentOrder().PopulateFrom(paymentOrder);
    }
}
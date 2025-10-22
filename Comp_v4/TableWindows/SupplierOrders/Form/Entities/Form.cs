using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Entities;

public class Form : GenericStateMachine<BaseFormState, Form>, ICreateSupplierOrdersHandler
{
    public Form(IEnumerable<BaseFormState> states, BaseFormState initialState) : base(states, initialState) {
        EventBus<ISupplierOrdersSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ISupplierOrdersSubscriber>.Unsubscribe(this);
    }

    public async Task OnCreateSupplierOrder(TaskCompletionSource tcs, object parameter = null) {
        await CurrentState.OnCreateSupplierOrder(this, tcs, parameter);
        tcs.TrySetResult();
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    protected readonly SupplierOrder _supplierOrder;
    protected readonly IRepository<SupplierOrder> _repository;

    protected BaseFormState(SupplierOrder supplierOrder, IRepository<SupplierOrder> repository) {
        _supplierOrder = supplierOrder;
        _repository = repository;
    }

    public abstract Task OnCreateSupplierOrder(Form form, TaskCompletionSource tcs, object parameter);
}

public class CreateFormState : BaseFormState
{
    public CreateFormState(SupplierOrder supplierOrder, IRepository<SupplierOrder> repository) : base(supplierOrder, repository) {
    }

    public override async Task OnCreateSupplierOrder(Form form, TaskCompletionSource tcs, object parameter) {
        await _repository.AddAsync(_supplierOrder);
    }
}
public class EditFormState : BaseFormState
{
    public EditFormState(SupplierOrder supplierOrder, IRepository<SupplierOrder> repository) : base(supplierOrder, repository) {
    }

    public override async Task OnCreateSupplierOrder(Form form, TaskCompletionSource tcs, object parameter) {
        await _repository.UpdateAsync(_supplierOrder);
    }
}
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Entities;

public class SoForm : GenericStateMachine<BaseSoFormState, SoForm>, ICreateSupplierOrdersHandler
{
    public SoForm(IEnumerable<BaseSoFormState> states, BaseSoFormState initialState) : base(states, initialState) {
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

public abstract class BaseSoFormState : StateBase<SoForm>
{
    protected readonly SupplierOrder _supplierOrder;
    protected readonly IRepository<SupplierOrder> _repository;

    protected BaseSoFormState(SupplierOrder supplierOrder, IRepository<SupplierOrder> repository) {
        _supplierOrder = supplierOrder;
        _repository = repository;
    }

    public abstract Task OnCreateSupplierOrder(SoForm form, TaskCompletionSource tcs, object parameter);
}

public class CreateSoFormState : BaseSoFormState
{
    public CreateSoFormState(SupplierOrder supplierOrder, IRepository<SupplierOrder> repository) : base(supplierOrder, repository) {
    }

    public override async Task OnCreateSupplierOrder(SoForm form, TaskCompletionSource tcs, object parameter) {
        await _repository.AddAsync(_supplierOrder);
    }
}
public class EditSoFormState : BaseSoFormState
{
    public EditSoFormState(SupplierOrder supplierOrder, IRepository<SupplierOrder> repository) : base(supplierOrder, repository) {
    }

    public override async Task OnCreateSupplierOrder(SoForm form, TaskCompletionSource tcs, object parameter) {
        await _repository.UpdateAsync(_supplierOrder);
    }
}
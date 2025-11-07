using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Entities;

public class SoForm : GenericStateMachine<BaseSoFormState, SoForm>, ICreateSupplierOrdersHandler, ISelectionConfirmationHandler
{
    public SoForm(IEnumerable<BaseSoFormState> states, BaseSoFormState initialState) : base(states, initialState) {
        EventBus<ISupplierOrdersSubscriber>.Subscribe(this);
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ISupplierOrdersSubscriber>.Unsubscribe(this);
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public async Task OnConfirmSelection(TaskCompletionSource tcs, object parameter = null) {
        await CurrentState.OnConfirmSelection(this, tcs, parameter);
        await tcs.Task;
    }

    public async Task OnCreateSupplierOrder(TaskCompletionSource tcs, object parameter = null) {
        //await CurrentState.OnCreateSupplierOrder(this, tcs, parameter);
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

    public virtual Task OnConfirmSelection(SoForm soForm, TaskCompletionSource tcs, object? parameter) {
        if (parameter is not Counterparty counterparty) 
            throw new InvalidCastException("parameter is not of type Counterparty");
        _supplierOrder.Counterparty = counterparty;
        tcs.TrySetResult();
        return Task.CompletedTask;
    }
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
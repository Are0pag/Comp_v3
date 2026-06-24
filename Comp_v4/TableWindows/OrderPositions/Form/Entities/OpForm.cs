using Comp_v4.TableWindows.OrderPositions.Events;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
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
    protected readonly IRepository<SupplierOrder> _supplierOrderRepository;
    protected readonly IRepository<Counterparty> _counterpartyRepository;
    protected readonly SoDataGridVm _soDataGridVm;

    protected BaseOpFormState(IRepository<OrderPosition> repository, IRepository<SupplierOrder> supplierOrderRepository, IRepository<Counterparty> counterpartyRepository, SoDataGridVm soDataGridVm) {
        _repository = repository;
        _supplierOrderRepository = supplierOrderRepository;
        _counterpartyRepository = counterpartyRepository;
        _soDataGridVm = soDataGridVm;
    }

    public virtual async Task Save(TaskCompletionSource tcs, OrderPosition item, object? args, OpForm opForm) {
        var targetSo = _soDataGridVm.Items.First(i => i.Id == item.SupplierOrder.Id);
        targetSo.Counterparty ??= await _counterpartyRepository.GetByIdAsync(item.SupplierOrder.CounterpartyId);
        targetSo.CopyTo(item.SupplierOrder);
        await _supplierOrderRepository.UpdateAsync(targetSo);
    }
    
    protected static async Task NotifyAboutSaving() {
        var savingTcs = new TaskCompletionSource();
        EventBus<IOrderPositionSubscriber>
           .RaiseEvent<IOrderPosSavingCommitHandler>(h => h?.OnSaveOp(savingTcs));
        await savingTcs.Task;
    }

    protected static void NotifyAboutUiChanging() {
        EventBus<IOrderPositionSubscriber>
           .RaiseEvent<IOpTableReloadHandler>(h => h?.OnOpTableReload());
    }
}

public class CreateOpFormState : BaseOpFormState
{
    public CreateOpFormState(IRepository<OrderPosition> repository, IRepository<SupplierOrder> supplierOrderRepository, IRepository<Counterparty> counterpartyRepository, SoDataGridVm soDataGridVm) : base(repository, supplierOrderRepository, counterpartyRepository, soDataGridVm) {
    }

    public override async Task Save(TaskCompletionSource tcs, OrderPosition item, object? args, OpForm opForm) {
        await base.Save(tcs, item, args, opForm);
        try {
            await _repository.AddAsync(item);
        }
        catch (Exception ex) {
            await Console.Error.WriteLineAsync(ex.Message);
            throw ex;
        }

        NotifyAboutUiChanging();
        await NotifyAboutSaving();

        tcs.TrySetResult();
    }
}

public class EditOpFormState : BaseOpFormState
{
    public EditOpFormState(IRepository<OrderPosition> repository, IRepository<SupplierOrder> supplierOrderRepository, IRepository<Counterparty> counterpartyRepository, SoDataGridVm soDataGridVm) : base(repository, supplierOrderRepository, counterpartyRepository, soDataGridVm) {
    }

    public override async Task Save(TaskCompletionSource tcs, OrderPosition item, object? args, OpForm opForm) {
        await base.Save(tcs, item, args, opForm);
        try {
            await _repository.UpdateAsync(item);
        }
        catch (Exception ex) {
            throw ex;
        }

        NotifyAboutUiChanging();
        await NotifyAboutSaving();

        tcs.TrySetResult();
    }
}
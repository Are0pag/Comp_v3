using Comp_v4.TableWindows.PaymentOrders.Form;
using Comp_v4.TableWindows.PaymentOrders.Form.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Actions;
using Comp.Db.Contracts;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Entities;

public class PaymentOrderTable : GenericStateMachine<PaymentOrderTableBaseState, PaymentOrderTable>
{
    public PaymentOrderTable(IEnumerable<PaymentOrderTableBaseState> states, PaymentOrderTableBaseState initialState) : base(states, initialState) {
        Console.WriteLine("fve");
    }

    public async Task AddItem(TaskCompletionSource tcs, PaymentOrder po, object? parameter = null) {
        await CurrentState.Add(this, tcs, po, parameter);
    }
    
    public async Task EditItem(TaskCompletionSource tcs, PaymentOrder po, object? parameter = null) {
        await CurrentState.Edit(this, tcs, po, parameter);
    }
    
    public async Task DeleteItem(TaskCompletionSource tcs, PaymentOrder po, object? parameter = null) {
        await CurrentState.Delete(this, tcs, po, parameter);
    }
}

public abstract class PaymentOrderTableBaseState : StateBase<PaymentOrderTable> 
{
    protected readonly IRepository<PaymentOrder> _repository;
    protected readonly IServiceProvider _serviceProvider;

    protected PaymentOrderTableBaseState(IRepository<PaymentOrder> repository, IServiceProvider serviceProvider) {
        _repository = repository;
        _serviceProvider = serviceProvider;
    }

    public async Task Add(PaymentOrderTable paymentOrderTable, TaskCompletionSource tcs, PaymentOrder po, object? parameter) {
        var window = ActivatorUtilities.CreateInstance<PaymentOrderFormWindow>(_serviceProvider, po);

        var form = _serviceProvider.GetRequiredService<PaymentOrderForm>();
        await form.ChangeState(form.GetState<CreatePoState>(), form);
        
        ResolveRelated();
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
    }

    public async Task Edit(PaymentOrderTable paymentOrderTable, TaskCompletionSource tcs, PaymentOrder po, object? parameter) {
        ArgumentNullException.ThrowIfNull(po);

        var window = ActivatorUtilities.CreateInstance<PaymentOrderFormWindow>(_serviceProvider, po);

        var form = _serviceProvider.GetRequiredService<PaymentOrderForm>();
        await form.ChangeState(form.GetState<EditPoState>(), form);
        
        ResolveRelated();
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
    }

    private void ResolveRelated() {
        
    }

    public async Task Delete(PaymentOrderTable paymentOrderTable, TaskCompletionSource tcs, PaymentOrder po, object? parameter) {
        await _repository.DeleteAsync(po.Id);
        tcs.TrySetResult();
    }
}

public class PaymentOrderTableInitialState : PaymentOrderTableBaseState
{
    public PaymentOrderTableInitialState(IRepository<PaymentOrder> repository, IServiceProvider serviceProvider) : base(repository, serviceProvider) {
    }
}
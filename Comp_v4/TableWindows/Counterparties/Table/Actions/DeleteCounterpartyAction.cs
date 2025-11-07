using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.Db.Contracts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class DeleteCounterpartyAction : BaseActionAsyncScopeHandler
{
    protected readonly CounterpartyDataGridVm _dataGridVm;
    protected readonly IRepository<Counterparty> _repository;
    protected readonly CounterpartyTableWindow _counterpartyTableWindow;
    
    public DeleteCounterpartyAction(DeleteCounterpartyButVm button, 
                                    IServiceScopeFactory scopeFactory, 
                                    CounterpartyDataGridVm dataGridVm, 
                                    IRepository<Counterparty> repository, 
                                    CounterpartyTableWindow counterpartyTableWindow) 
        : base(button, scopeFactory) {
        _dataGridVm = dataGridVm;
        _repository = repository;
        _counterpartyTableWindow = counterpartyTableWindow;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _repository.DeleteAsync(_dataGridVm.SelectedItem!.Id);
        tcs.TrySetResult();
        _counterpartyTableWindow.OnReload?.Invoke();
    }
    
    public override bool CanPerform() {
        return base.CanPerform() && _dataGridVm.SelectedItem != null;
    }
}
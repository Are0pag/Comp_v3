using Comp_v4.TableWindows.Counterparties.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Templates.Common;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.Counterparties.Table.Vm;

public class CounterpartyDataGridVm : CollectionViewModel<Counterparty>, ISaveHandler
{
    public CounterpartyDataGridVm(IRepository<Counterparty> repository) : base(repository) {
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public async Task Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null) {
        if (parameter is not Counterparty counterparty) 
            throw new InvalidCastException();

        if (Items.All(c => c.Id != counterparty.Id)) {
            Items.Add(counterparty);
            OnPropertyChanged(nameof(Items));
        }
        tcs.SetResult(counterparty);
    }
}
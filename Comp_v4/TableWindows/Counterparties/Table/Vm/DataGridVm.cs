using Comp_v4.TableWindows.Counterparties.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.Counterparties.Table.Vm;

public class DataGridVm : DataGridViewModel<Counterparty>, ISaveHandler
{
    public DataGridVm(IRepository<Counterparty> repository) : base(repository) {
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public async Task Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null) {
        if (parameter is not Counterparty counterparty) 
            throw new InvalidCastException();
        
        Items.Add(counterparty);
        OnPropertyChanged(nameof(Items));
        tcs.SetResult(counterparty);
    }
}
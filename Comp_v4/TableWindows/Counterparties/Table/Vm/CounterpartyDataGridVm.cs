using System.Collections.ObjectModel;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Templates.Common;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.Counterparties.Table.Vm;

public class CounterpartyDataGridVm : FilterGridVm<Counterparty>, ISaveHandler
{
    public CounterpartyDataGridVm(IRepository<Counterparty> repository, IFilter<Counterparty, FiltersVmBase> filter) : base(repository, filter) {
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public override async Task InitFilteringCollection() {
        var data = await _repository.GetAllAsync();
        Items = new ObservableCollection<Counterparty>(data);
        ItemsSorted = new ObservableCollection<Counterparty>(Items);
        
        OnPropertyChanged(nameof(ItemsSorted));
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
        else {
            try {
                if (Items.First(i => i.Id == counterparty.Id) is { } sourceItem) {
                    sourceItem.PopulateFrom(counterparty);
                    OnPropertyChanged(nameof(Items));
                }
                    
            }
            catch (InvalidOperationException ex) {
                throw;
            }
        }
        
        tcs.SetResult(counterparty);
    }

}
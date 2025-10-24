using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Counterparties.Form.Vm;

public class CounterpartyEnumsVm : ObservableObject
{
    protected readonly Counterparty _counterparty;
    private CounterpartyType _counterpartyType = CounterpartyType.Supplier;

    public CounterpartyEnumsVm(Counterparty counterparty) {
        _counterparty = counterparty;
    }

    public ObservableCollection<CounterpartyType> CounterpartyTypes { get; } 
        = new(Enum.GetValues(typeof(CounterpartyType)).Cast<CounterpartyType>());

    public CounterpartyType CounterpartyType {
        get => _counterpartyType;
        set {
            SetProperty(ref _counterpartyType, value);
            _counterparty.CounterpartyTypeName = value.ToString();
        }
    }
}
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Counterparties.Form.Vm;

public class CounterpartyEnumsVm : ObservableObject
{
    public ObservableCollection<CounterpartyType> CounterpartyTypes { get; } 
        = new(Enum.GetValues(typeof(CounterpartyType)).Cast<CounterpartyType>());

    private CounterpartyType _counterpartyType = CounterpartyType.Supplier;
    public CounterpartyType CounterpartyType 
    {
        get => _counterpartyType;
        set => SetProperty(ref _counterpartyType, value);
    }
}
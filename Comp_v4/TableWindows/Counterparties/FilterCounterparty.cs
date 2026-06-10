using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.Counterparties;

public class FilterCounterparty : IFilter<Counterparty, FiltersVmBase>
{
    public Func<Counterparty, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }

    public FilterCounterparty() {
        ApplyFilter = (counterparty, vm, comparison) => {
            if (String.IsNullOrEmpty(vm.FilterString))
                return true;

            if (counterparty.ShortName.Contains(vm.FilterString, comparison)) 
                return true;
            
            if (counterparty.FullName != null && counterparty.FullName.Contains(vm.FilterString, comparison)) 
                return true;
            
            return false;
        };
    }
}
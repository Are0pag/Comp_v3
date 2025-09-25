using Comp_v4.Operations.Commands.Filtering;
using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;

public class Filter : IFilter<ConditionalDesignation, FiltersVmBase>
{
    public Filter() {
        ApplyFilter = (item, vm, comparisonType ) =>
            (string.IsNullOrEmpty(vm.FilterString) || item.Designation.Contains(vm.FilterString, comparisonType)) ||
             item.Name.Contains(vm.FilterString, comparisonType);
    }

    public Func<ConditionalDesignation, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }
}
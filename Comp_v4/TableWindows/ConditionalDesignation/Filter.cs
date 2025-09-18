using Comp_v4.Operations.Commands.Filtering;
using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;

public class Filter : IFilter<ConditionalDesignation, FiltersVmCd>
{
    public Filter() {
        ApplyFilter = (item, vm, comparisonType ) =>
            (string.IsNullOrEmpty(vm.FilterDesignation) ||
             item.Designation.Contains(vm.FilterDesignation, comparisonType)) &&
            (string.IsNullOrEmpty(vm.FilterName) ||
             item.Name.Contains(vm.FilterName, comparisonType));
    }

    public Func<ConditionalDesignation, FiltersVmCd, StringComparison, bool> ApplyFilter { get; init; }
}
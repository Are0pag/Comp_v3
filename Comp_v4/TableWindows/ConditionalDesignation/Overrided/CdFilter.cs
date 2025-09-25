using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.ConditionalDesignation.Overrided;

public class CdFilter : IFilter<Comp.ModelData.TechnicalItems.ConditionalDesignation, FiltersVmBase>
{
    public CdFilter() {
        ApplyFilter = (item, vm, comparisonType ) =>
            (string.IsNullOrEmpty(vm.FilterString) || item.Designation.Contains(vm.FilterString, comparisonType)) ||
             item.Name.Contains(vm.FilterString, comparisonType);
    }

    public Func<Comp.ModelData.TechnicalItems.ConditionalDesignation, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }
}
using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.MeasurementUnits;

public class muFilter : IFilter<MeasurementUnit, FiltersVmBase>
{
    public muFilter() {
        ApplyFilter = (item, vm, comparison) => {
            return string.IsNullOrEmpty(vm.FilterString) 
                   || item.Designation.Contains(vm.FilterString, comparison) 
                   || item.Name.Contains(vm.FilterString, comparison);
        };
    }
    
    public Func<MeasurementUnit, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }
}
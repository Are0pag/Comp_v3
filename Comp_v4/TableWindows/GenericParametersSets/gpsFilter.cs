using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.GenericParametersSets;

public class gpsFilter : IFilter<GenericParametersSet, FiltersVmBase>
{
    public gpsFilter() {
        ApplyFilter = (item, vm, comparison) => {
            return string.IsNullOrEmpty(vm.FilterString) 
                   || item.GpMain.Contains(vm.FilterString, comparison)
                   || item.Gp1.Contains(vm.FilterString, comparison)
                   || item.Gp2.Contains(vm.FilterString, comparison)
                   || item.Gp3.Contains(vm.FilterString, comparison)
                   || item.Gp4.Contains(vm.FilterString, comparison)
                   || item.Gp5.Contains(vm.FilterString, comparison)
                   || item.Name.Contains(vm.FilterString, comparison);
        };
    }
    
    public Func<GenericParametersSet, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }
}
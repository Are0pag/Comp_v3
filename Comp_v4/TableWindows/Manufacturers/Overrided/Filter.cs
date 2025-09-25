using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.Manufacturers.Overrided;

public class Filter : IFilter<Manufacturer, FiltersVmBase>
{
    public Filter() {
        ApplyFilter = (item, vm, comparison) => {
            return string.IsNullOrEmpty(vm.FilterString) 
                   || item.Designation.Contains(vm.FilterString, comparison) 
                   || item.FullName.Contains(vm.FilterString, comparison)
                   || item.Url.Contains(vm.FilterString, comparison)
                   || item.Remark.Contains(vm.FilterString, comparison);
        };
    }
    public Func<Manufacturer, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }
}
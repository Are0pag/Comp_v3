using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.TypeSizes;

public class tsFilter : IFilter<TypeSize, FiltersVmBase>
{
    public tsFilter() {
        ApplyFilter = (item, vm, comparison) => {
            return string.IsNullOrEmpty(vm.FilterString) 
                   || item.Designation.Contains(vm.FilterString, comparison) 
                   || item.Description.Contains(vm.FilterString, comparison);
        };
    }
    public Func<TypeSize, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }
}
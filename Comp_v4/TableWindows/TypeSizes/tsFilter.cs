using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.TypeSizes;

public class tsFilter : IFilter<TypeSize, FiltersVmBase>
{
    public tsFilter() {
        ApplyFilter = (item, vm, comparison) => 
            string.IsNullOrEmpty(vm.FilterString) 
            || item.Designation.Contains(vm.FilterString, comparison) 
            || item.Description.Contains(vm.FilterString, comparison)
            || (int.TryParse(vm.FilterString, out int num) && item.OutputsNumber == num)
            || ((vm.FilterString.Equals("да", comparison) && item.IsUsingSmd) || (vm.FilterString.Equals("нет", comparison) && !item.IsUsingSmd));

        // ApplyFilter = (item, vm, comparison) => 
        //     string.IsNullOrEmpty(vm.FilterString) 
        //     || item.Designation.Contains(vm.FilterString, comparison) 
        //     || item.Description.Contains(vm.FilterString, comparison)
        //     || (int.TryParse(vm.FilterString, out int num) && item.OutputsNumber == num)
        //     || ((vm.FilterString == "да" && item.IsUsingSmd) || (vm.FilterString == "нет" && !item.IsUsingSmd))
        //     || ((vm.FilterString == "Да" && item.IsUsingSmd) || (vm.FilterString == "Нет" && !item.IsUsingSmd))
        //     ;
    }
    public Func<TypeSize, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }
}
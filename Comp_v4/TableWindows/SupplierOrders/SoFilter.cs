using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.SupplierOrders;

public class SoFilter : IFilter<SupplierOrder, FiltersVmBase>
{
    public Func<SupplierOrder, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }

    public SoFilter() {
        ApplyFilter = (item, source, comparison) 
            => String.IsNullOrEmpty(source.FilterString)
               || item.PurchaseOrderNumber.Contains(source.FilterString, comparison);
    }
}
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.PaymentOrders.Table;

public class PoFilter : IFilter<PaymentOrder, FiltersVmBase>
{
    public Func<PaymentOrder, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }

    public PoFilter() {
        ApplyFilter = (item, source, comparison) => {
            return String.IsNullOrEmpty(source.FilterString)
                   || item.Number.Contains(source.FilterString, comparison);
        };
    }
}
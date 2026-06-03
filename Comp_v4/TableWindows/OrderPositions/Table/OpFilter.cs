using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.OrderPositions.Table;

public class OpFilter : IFilter<OrderPosition, FiltersVmBase>
{
    public Func<OrderPosition, FiltersVmBase, StringComparison, bool> ApplyFilter { get; init; }

    public OpFilter() {
        ApplyFilter = (item, source, comparison) => {
            return String.IsNullOrEmpty(source.FilterString)
                   || item.Position.Name.Contains(source.FilterString, comparison);
        };
    }
}
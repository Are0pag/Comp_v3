using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4.Operations.Commands.Filtering;

public interface IFilter<T, TSource> 
    where T : class, IDbEntity
    where TSource : FiltersVmBase
{
    Func<T, TSource, StringComparison, bool> ApplyFilter { get; init; }
}
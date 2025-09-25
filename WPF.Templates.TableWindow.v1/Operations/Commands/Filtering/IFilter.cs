using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;

public interface IFilter<T, TSource> 
    where T : class, IDbEntity
    where TSource : FiltersVmBase
{
    Func<T, TSource, StringComparison, bool> ApplyFilter { get; init; }
}
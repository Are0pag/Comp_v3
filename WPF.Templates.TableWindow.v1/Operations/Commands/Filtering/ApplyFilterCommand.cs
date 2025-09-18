using System.Collections.ObjectModel;
using System.Windows;
using Comp.ModelData.TechnicalItems;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4.Operations.Commands.Filtering;

public class ApplyFilterCommand<TWindow, T, TFiltersSource> : BaseCommand<ApplyFilterCommand<TWindow, T, TFiltersSource>.Args>
    where TWindow : Window
    where T : class, IDbEntity
    where TFiltersSource : FiltersVmBase
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;
    protected readonly Args _args;
    protected readonly List<T> _removedByLastFilter = new ();
    protected readonly IFilter<T, TFiltersSource> _filter;
    
    public ApplyFilterCommand(Args parameter, ModuleContext<TWindow, T> moduleContext, IFilter<T, TFiltersSource> filter) : base(parameter) {
        _args = parameter;
        _moduleContext = moduleContext;
        _filter = filter;
    }

    public override Task ExecuteAsync() {
        var comparisonType = _args.Filters.IgnoreCase 
            ? StringComparison.OrdinalIgnoreCase 
            : StringComparison.Ordinal;

        var sorted = _args.Items.Where(item => _filter.ApplyFilter(item, _args.Filters, comparisonType)).ToList();

        foreach (var item in _args.Items) {
            if (sorted.Contains(item)) continue;
            _removedByLastFilter.Add(item);
        }

        foreach (var removed in _removedByLastFilter) 
            _args.Items.Remove(removed);
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        foreach (var item in _removedByLastFilter) 
            _args.Items.Add(item);
        return Task.CompletedTask;
    }


    public class Args(ObservableCollection<T> items, TFiltersSource filters)
    {
        public ObservableCollection<T> Items { get; set; } = items;
        public TFiltersSource Filters { get; set; } = filters;
    }
}
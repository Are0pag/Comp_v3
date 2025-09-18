using System.Collections.ObjectModel;
using System.Windows;
using Comp.ModelData.TechnicalItems;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4.Operations.Commands.Filtering;

public interface IFilter<T> where T : class, IDbEntity
{
    Func<T, bool> ApplyFilter { get; init; }
}

public class Filter : IFilter<ConditionalDesignation>
{
    public Filter() {
        ApplyFilter = x => true;
    }
    
    public Func<ConditionalDesignation, bool> ApplyFilter { get; init; }
}

public class ApplyFilterCommand<TWindow, T> : BaseCommand<ApplyFilterCommand<TWindow, T>.Args>
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;
    protected readonly Args _args;
    protected readonly List<T> _removedByLastFilter = new ();
    
    public ApplyFilterCommand(Args parameter, ModuleContext<TWindow, T> moduleContext) : base(parameter) {
        _args = parameter;
        _moduleContext = moduleContext;
    }

    public override Task ExecuteAsync() {
        var comparisonType = _args.Filters.IgnoreCase 
            ? StringComparison.OrdinalIgnoreCase 
            : StringComparison.Ordinal;

        var sorted = _args.Items.Where(item =>
                                             (string.IsNullOrEmpty(_args.Filters.FilterDesignation) ||
                                              item.Designation.Contains(_args.Filters.FilterDesignation, comparisonType)) &&
                                             (string.IsNullOrEmpty(_args.Filters.FilterName) ||
                                              item.Name.Contains(_args.Filters.FilterName, comparisonType))
                                        ).ToList();

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


    public class Args(ObservableCollection<T> items, FiltersVm filters)
    {
        public ObservableCollection<T> Items { get; set; } = items;
        public FiltersVm Filters { get; set; } = filters;
    }
}
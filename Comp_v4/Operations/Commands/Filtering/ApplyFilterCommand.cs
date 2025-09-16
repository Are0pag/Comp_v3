using System.Collections.ObjectModel;
using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4.Operations.Commands.Filtering;

public class SortingObservableCollection<T> : ObservableCollection<T> 
{
    public void AddAndNotify(T item) {
        Add(item);
        
    }
}

public class FilteringView<T>
{
    public ObservableCollection<T> Origin { get; protected set; } = new();
    public List<T> Sorted { get; protected set; } = new();

    public void Add(T item) {
        Origin.Add(item);
        Sorted.Add(item);
    }

    public void Remove(T item) {
        Origin.Remove(item);
        Sorted.Remove(item);
    }
}

public class ApplyFilterCommand : BaseCommand<ApplyFilterCommand.Args>
{
    protected readonly Args _args;
    protected List<ConditionalDesignation>? _sortedItems;
    
    public ApplyFilterCommand(Args parameter) : base(parameter) {
        _args = parameter;
    }
    
    public ObservableCollection<ConditionalDesignation> Origin { get; protected set; } = [];

    public override Task ExecuteAsync() {
        _sortedItems = _args.Items.Where(item =>
                                                                     (string.IsNullOrEmpty(_args.Filters.FilterDesignation) ||
                                                                      item.Designation.Contains(_args.Filters.FilterDesignation)) &&
                                                                     (string.IsNullOrEmpty(_args.Filters.FilterName) ||
                                                                      item.Name.Contains(_args.Filters.FilterName))
                                                                ).ToList();
        _moduleContext.DataGrid.ItemsSource = _sortedItems;
        Origin = new ObservableCollection<ConditionalDesignation>(_args.Items);
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _moduleContext.DataGrid.ItemsSource = _args.Items;
        return Task.CompletedTask;
    }


    public class Args(ObservableCollection<ConditionalDesignation> items, FiltersVm filters)
    {
        public ObservableCollection<ConditionalDesignation> Items { get; set; } = items;
        public FiltersVm Filters { get; set; } = filters;
    }
}
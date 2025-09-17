using System.Collections.ObjectModel;
using Comp.ModelData.TechnicalItems;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4.Operations.Commands.Filtering;

public class ApplyFilterCommand : BaseCommand<ApplyFilterCommand.Args>
{
    protected readonly ModuleContext _moduleContext;
    protected readonly Args _args;
    protected List<ConditionalDesignation>? _sortedItems;
    
    public ApplyFilterCommand(Args parameter, ModuleContext moduleContext) : base(parameter) {
        _args = parameter;
        _moduleContext = moduleContext;
    }

    public override Task ExecuteAsync() {
        var comparisonType = _args.Filters.IgnoreCase 
            ? StringComparison.OrdinalIgnoreCase 
            : StringComparison.Ordinal;

        _sortedItems = _args.Items.Where(item =>
                                             (string.IsNullOrEmpty(_args.Filters.FilterDesignation) ||
                                              item.Designation.Contains(_args.Filters.FilterDesignation, comparisonType)) &&
                                             (string.IsNullOrEmpty(_args.Filters.FilterName) ||
                                              item.Name.Contains(_args.Filters.FilterName, comparisonType))
                                        ).ToList();
        
        _moduleContext.DataGrid.ItemsSource = _sortedItems;
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
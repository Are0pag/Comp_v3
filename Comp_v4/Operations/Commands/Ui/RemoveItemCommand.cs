using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RemoveItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly ModuleContext _context;
    public RemoveItemCommand(ConditionalDesignation parameter, ModuleContext context) : base(parameter) {
        _context = context;
    }

    public override Task ExecuteAsync() {
        if (_context.DataGrid.ItemsSource != _context.DataGridViewModel.Items) {
            _context.DataGrid.ItemsSource = _context.DataGridViewModel.Items;
        }
        
        _context.DataGridViewModel.Items.Remove(_parameter);
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.DataGridViewModel.Items.Add(_parameter);
        _context.DataGrid.ScrollIntoView(_parameter);
        return Task.CompletedTask;
    }
}
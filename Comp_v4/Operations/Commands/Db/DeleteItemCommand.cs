using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class DeleteItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly IRepository<ConditionalDesignation> _repository;
    public DeleteItemCommand(ConditionalDesignation parameter) : base(parameter) {
        _repository = App.Host.Services.GetRequiredService<IRepository<ConditionalDesignation>>();
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.DeleteAsync(_parameter.Id);
    }
}

public class RemoveItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly ModuleContext _context;
    public RemoveItemCommand(ConditionalDesignation parameter) : base(parameter) {
        _context = App.Host.Services.GetRequiredService<ModuleContext>();
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
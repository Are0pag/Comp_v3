using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm;

namespace Comp_v4.Operations.Commands;

public class CreateRawCommand : BaseCommand<object>
{
    protected readonly ModuleContext _context;
    public CreateRawCommand(object parameter) : base(parameter) {
        _context = App.Host.Services.GetRequiredService<ModuleContext>();
    }
    public ConditionalDesignation Item { get; protected set; }

    public override Task ExecuteAsync() {
        try {
            Item = new ConditionalDesignation("", "");
            _context.DataGridViewModel.Items.Add(Item);
            ((DataGridViewModelFiltered)_context.DataGridViewModel).RefreshFilters();
            _context.DataGridViewModel.SelectedItem = Item;
        }
        catch (Exception e) {
            e.Log(this);
            throw;
        }
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.DataGridViewModel.Items.Remove(Item);
        ((DataGridViewModelFiltered)_context.DataGridViewModel).RefreshFilters();
        return Task.CompletedTask;
    }
}
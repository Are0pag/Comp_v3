using System.Collections.ObjectModel;
using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class CreateRawCommand : BaseCommand<ModuleContext>
{
    protected readonly ModuleContext _context;
    public CreateRawCommand(ModuleContext parameter) : base(parameter) {
        _context = parameter;
    }
    public ConditionalDesignation Item { get; protected set; }

    public override Task ExecuteAsync() {
        try {
            Item = new ConditionalDesignation("", "");
            /*var currentDataSource = (ObservableCollection<ConditionalDesignation>)_context.DataGrid.ItemsSource;
            currentDataSource.Add(Item);*/
            _context.DataGridViewModel.Items.Add(Item);
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
        return Task.CompletedTask;
    }
}
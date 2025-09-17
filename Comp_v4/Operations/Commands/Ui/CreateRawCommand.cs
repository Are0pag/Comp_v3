using Comp.ModelData.TechnicalItems;
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
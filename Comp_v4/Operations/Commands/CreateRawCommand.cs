using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class CreateRawCommand : BaseCommand<object>
{
    protected readonly ModuleContext _context;
    public CreateRawCommand(object? parameter, ModuleContext context) : base(parameter) {
        _context = context;
    }

    public ConditionalDesignation Item { get; protected set; }

    public override Task ExecuteAsync() {
        _item = new ConditionalDesignation("", "");
        _context.DataGridViewModel.Items.Add(_item);
        _context.DataGridViewModel.SelectedItem = _item;
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.DataGridViewModel.Items.Remove(_item);
        return Task.CompletedTask;
    }
}
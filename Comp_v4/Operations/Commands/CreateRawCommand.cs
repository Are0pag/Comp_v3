using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class CreateRawCommand : BaseCommand
{
    public CreateRawCommand(ModuleContext context, object? parameter = null) : base(context, parameter) {
    }

    public ConditionalDesignation Item { get; protected set; }

    public override Task ExecuteAsync() {
        _item = new ConditionalDesignation("", "");
        _context.DataGridViewModel.Items.Add(_item);
        _context.DataGridViewModel.SelectedItem = _item;
        Item = _item;
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.DataGridViewModel.Items.Remove(_item);
        return Task.CompletedTask;
    }
}
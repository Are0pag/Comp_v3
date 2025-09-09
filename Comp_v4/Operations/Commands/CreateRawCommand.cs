using Comp.ModelData.TechnicalItems;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberSelection : BaseCommand
{
    public RememberSelection(ModuleContext context) : base(context) {
    }

    public override async Task ExecuteAsync() {
        _item = _context.DataGridViewModel.SelectedItem;
    }

    public override async Task UndoAsync() {
        _context.DataGridViewModel.SelectedItem = _item;
    }

    public override async Task ExecuteDeferredAsync() {
       
    }
}

public class CreateRawCommand : BaseCommand
{
    public CreateRawCommand(ModuleContext context) : base(context) {
    }

    public override Task ExecuteAsync() {
        _item = new ConditionalDesignation("", "");
        _context.DataGridViewModel.Items.Add(_item);
        _context.DataGridViewModel.SelectedItem = _item;
        return Task.CompletedTask;
    }

    public override async Task UndoAsync() {
        _context.DataGridViewModel.Items.Remove(_item);
    }

    public override async Task ExecuteDeferredAsync() {
        throw new NotImplementedException();
    }
}
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberSelectionCommand : BaseCommand
{
    public RememberSelectionCommand(ModuleContext context) : base(context) {
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
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberSelectionCommand : BaseCommand<object>
{
    public RememberSelectionCommand(object? parameter = null) : base(parameter) {
    }

    /*public override Task ExecuteAsync() {
        _item = _context.DataGridViewModel.SelectedItem;
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.DataGridViewModel.SelectedItem = _item;
        _context.DataGrid.Focus();
        return Task.CompletedTask;
    }*/
}
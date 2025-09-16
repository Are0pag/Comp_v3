using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberSelectionCommand : BaseCommand<object>
{
    public RememberSelectionCommand(object? parameter = null) : base(parameter) {
    }

    public override Task ExecuteAsync() {
        _item = _moduleContext.DataGridViewModel.SelectedItem;
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _moduleContext.DataGridViewModel.SelectedItem = _item;
        _moduleContext.DataGrid.Focus();
        return Task.CompletedTask;
    }
}
using Comp.ModelData.TechnicalItems;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberSelectionCommand : BaseCommand<object>
{
    protected readonly ModuleContext _moduleContext;
    protected ConditionalDesignation? _item;
    
    public RememberSelectionCommand(object parameter, ModuleContext moduleContext) : base(parameter) {
        _moduleContext = moduleContext;
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
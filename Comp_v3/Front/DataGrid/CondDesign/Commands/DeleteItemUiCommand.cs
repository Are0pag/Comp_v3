using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochronous;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class DeleteItemUiCommand : IUiCommand
{
    private readonly CognDesignGridVm _vm;
    private object _backupItem;
    
    public string Description => "Remove from UI";

    public DeleteItemUiCommand(CognDesignGridVm vm) => _vm = vm;

    public void ExecuteUi() {
        _backupItem = _vm.SelectedItem;
        _vm.Items.Remove(_vm.SelectedItem);
        _vm.SelectedItem = null;
    }

    public void UndoUi() {
        _vm.Items.Add((ConditionalDesignation) _backupItem);
        _vm.SelectedItem = (ConditionalDesignation)_backupItem;
    }

    // Для интерфейса ICommand
    public Task ExecuteAsync() { ExecuteUi(); return Task.CompletedTask; }
    public Task UndoAsync() { UndoUi(); return Task.CompletedTask; }
}
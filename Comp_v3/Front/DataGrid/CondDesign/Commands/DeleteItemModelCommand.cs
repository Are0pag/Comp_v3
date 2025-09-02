using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Infrastructure.Command.Heterochronous;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class DeleteItemModelCommand : IModelCommand
{
    private readonly CognDesignGridVm _vm;
    
    public DeleteItemModelCommand(CognDesignGridVm vm) => _vm = vm;

    public async Task<bool> ValidateAsync() => _vm.SelectedItem != null;
    public async Task ExecuteAsync() => await _vm.Repository.DeleteAsync(_vm.SelectedItem.Id);
    public async Task UndoAsync() => throw new NotImplementedException();
    public bool Validate() {
        throw new NotImplementedException();
    }
}
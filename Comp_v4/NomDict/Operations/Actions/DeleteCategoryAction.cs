using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;
using Utils.WPF;

namespace Comp_v4.NomDict.Entities;

public class DeleteCategoryAction : BaseAsyncActionButtonInvoked
{
    protected readonly TreeViewVm _treeViewVm;
    protected readonly IRepository<Category> _repository;
    
    public DeleteCategoryAction(DeleteCategoryButtonVm buttonVm, TreeViewVm treeViewVm, IRepository<Category> repository) : base(buttonVm) {
        _treeViewVm = treeViewVm;
        _repository = repository;
    }

    public override async Task PerformAsync(object? parameter) {
        var deletingCategory = _treeViewVm.SelectedCategory!;

        _treeViewVm.SelectedCategory = null;
        _treeViewVm.Items.Remove(deletingCategory);
        await _repository.DeleteAsync(deletingCategory.Id);
        
        _treeViewVm.NotifyUiForChanges();
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp.Db;
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
        
        await DeleteCategoryRecursiveAsync(deletingCategory);

        _treeViewVm.SelectedCategory = null;
        
        if (deletingCategory.ParentCategory == null) {
            _treeViewVm.Items.Remove(deletingCategory);
        }
        else {
            deletingCategory.ParentCategory.RemoveSubcategory(deletingCategory);
        }
        _treeViewVm.NotifyUiForChanges();
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory is { Name: not DatabaseInitializer.ROOT_CATEGORY_NAME };
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }

    private async Task DeleteCategoryRecursiveAsync(Category category) {
        foreach (var subcategory in category.Subcategories.ToList()) 
            await DeleteCategoryRecursiveAsync(subcategory);

        await _repository.DeleteAsync(category.Id);
    }
}
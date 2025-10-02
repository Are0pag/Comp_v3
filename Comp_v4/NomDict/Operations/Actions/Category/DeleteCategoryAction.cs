using System.Collections.ObjectModel;
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
        if (!CanPerform()) return;

        var deletingCategoryFromDb = await _repository.GetByIdAsync(_treeViewVm.SelectedCategory!.Id);
        if (deletingCategoryFromDb == null) 
            return;

        // Получаем родителя из БД если есть
        Category? parentFromDb = null;
        if (deletingCategoryFromDb.ParentCategoryId.HasValue)
            parentFromDb = await _repository.GetByIdAsync(deletingCategoryFromDb.ParentCategoryId.Value);

        // Рекурсивное удаление из БД
        await DeleteCategoryRecursiveAsync(deletingCategoryFromDb);

        // Обновляем UI
        _treeViewVm.SelectedCategory = null;

        if (parentFromDb == null) {
            // Удаляем из корневой коллекции UI
            var itemToRemove = _treeViewVm.Items.FirstOrDefault(x => x.Id == deletingCategoryFromDb.Id);
            if (itemToRemove != null)
                _treeViewVm.Items.Remove(itemToRemove);
        }
        else {
            // Обновляем родителя в UI
            var parentInUi = FindCategoryInTree(parentFromDb.Id, _treeViewVm.Items);
            parentInUi?.RemoveSubcategory(deletingCategoryFromDb);
        }

        _treeViewVm.NotifyUiForChanges();
    }

    public override bool CanPerform() {
        var canPerform = _treeViewVm.SelectedCategory is { Name: not DatabaseInitializer.ROOT_CATEGORY_NAME, Subcategories.Count: 0 };
        return canPerform;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }

    private async Task DeleteCategoryRecursiveAsync(Category category) {
        foreach (var subcategory in category.Subcategories.ToList()) 
            await DeleteCategoryRecursiveAsync(subcategory);

        await _repository.DeleteAsync(category.Id);
    }

    private Category? FindCategoryInTree(int categoryId, ObservableCollection<Category> categories) {
        foreach (var category in categories) {
            if (category.Id == categoryId) 
                return category;
            var found = FindCategoryInTree(categoryId, category.Subcategories);
            if (found != null) 
                return found;
        }
        return null;
    }
}
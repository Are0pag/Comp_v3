using System.Collections.ObjectModel;
using Comp_v4.NomDict.Vm;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;

namespace Comp_v4.NomDict.Entities;

public class MoveCategoryAction
{
    protected readonly TreeViewVm _treeViewVm;
    protected readonly IRepository<Category> _repository;
    protected bool _isProcessing = false;

    public MoveCategoryAction(TreeViewVm treeViewVm, IRepository<Category> repository) {
        _treeViewVm = treeViewVm;
        _repository = repository;
    }

    public async Task Do(int i) {
        if (i == 5)
            return;
        await _repository.GetByIdAsync(i);
    }
    
    public async Task PerformAsync(Category sourceCategory, Category targetCategory) {
        if (sourceCategory == null || targetCategory == null || _isProcessing) 
            return;

        // Проверка на циклические ссылки
        if (IsChildOf(sourceCategory, targetCategory)) 
            return;
        
        // Загружаем сущности из БД чтобы гарантировать, что работаем с отслеживаемыми экземплярами
        var sourceFromDb = await _repository.GetByIdAsync(sourceCategory.Id);
        var targetFromDb = await _repository.GetByIdAsync(targetCategory.Id);
        
        if (sourceFromDb == null || targetFromDb == null)
            return;
        
        var prevOwner = sourceFromDb.ParentCategoryId.HasValue 
            ? await _repository.GetByIdAsync(sourceFromDb.ParentCategoryId.Value)
            : null;
        
        // Проверяем, не пытаемся ли переместить в того же родителя
        if (sourceFromDb.ParentCategoryId == targetFromDb.Id) 
            return;
        
        if (prevOwner == null) {
            if (sourceCategory.ParentCategory != null)
                return;
        }
        else {
            prevOwner.Subcategories.Remove(sourceCategory);
        }
        targetCategory.AddSubcategory(sourceCategory);
        
        sourceFromDb.ParentCategory = targetFromDb;
        
        targetCategory.IsExpanded = true;
        targetFromDb.IsExpanded = true;

        _isProcessing = true;
        await _repository.UpdateAsync(sourceFromDb);
        await _repository.UpdateAsync(targetFromDb);
        _isProcessing = false;

        _treeViewVm.NotifyUiForChanges();
    }

    /// <summary>
    /// Проверка на циклические ссылки
    /// </summary>
    private bool IsChildOf(Category parent, Category child) {
        if (child.ParentCategory == null) 
            return false;
        return child.ParentCategory == parent || IsChildOf(parent, child.ParentCategory);
    }

    private Category? FindParentCategory(Category sourceCategory, ObservableCollection<Category> items) {
        foreach (var item in items) {
            // Проверяем непосредственные подкатегории
            if (item.Subcategories.Contains(sourceCategory)) return item;

            // Рекурсивный поиск во вложенных подкатегориях
            var nestedParent = FindParentCategoryRecursive(sourceCategory, item.Subcategories);
            if (nestedParent != null) return nestedParent;
        }

        return null;
    }

    private Category? FindParentCategoryRecursive(Category sourceCategory, ObservableCollection<Category> categories) {
        foreach (var category in categories) {
            if (category.Subcategories.Contains(sourceCategory)) return category;

            var nestedParent = FindParentCategoryRecursive(sourceCategory, category.Subcategories);
            if (nestedParent != null) return nestedParent;
        }

        return null;
    }

}
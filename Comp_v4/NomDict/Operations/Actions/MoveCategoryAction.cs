using System.Collections.ObjectModel;
using Comp_v4.NomDict.Vm;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;

namespace Comp_v4.NomDict.Entities;

public class MoveCategoryAction
{
    protected readonly TreeViewVm _treeViewVm;
    protected readonly IRepository<Category> _repository;

    public MoveCategoryAction(TreeViewVm treeViewVm, IRepository<Category> repository) {
        _treeViewVm = treeViewVm;
        _repository = repository;
    }

    public async Task PerformAsync(Category sourceCategory, Category targetCategory) {
        if (sourceCategory == null || targetCategory == null) return;

        // Проверка на циклические ссылки
        if (IsChildOf(sourceCategory, targetCategory)) return;

        var prevOwner = _treeViewVm.Items.Where(c => c.Subcategories.Contains(sourceCategory)).ToList();
        if (prevOwner.Count != 1) {
            if (sourceCategory.ParentCategory != null)
                throw new Exception();
        }
        else {
            prevOwner[0].Subcategories.Remove(sourceCategory);
        }
        
        targetCategory.AddSubcategory(sourceCategory);
        sourceCategory.ParentCategory = targetCategory;
        
        targetCategory.IsExpanded = true;

        await _repository.UpdateAsync(sourceCategory);

        _treeViewVm.NotifyUiForChanges();
    }

    private bool IsChildOf(Category parent, Category child) {
        if (child.ParentCategory == null) return false;
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
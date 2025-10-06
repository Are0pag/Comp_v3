using Comp.ModelData.SortingItems;

namespace Comp.Db.Repositories.Concrete;

public class RepositoryCategory : DbRepository<Category>
{
    public RepositoryCategory(AppDbContext context) : base(context) {
    }

    public override async Task<List<Category>> GetAllAsync() {
        var items = await base.GetAllAsync();
        var categoriesByParent = items
                                .Where(c => c.ParentCategoryId.HasValue)
                                .GroupBy(c => c.ParentCategoryId!.Value)
                                .ToDictionary(g => g.Key, g => g.ToList());

        // Заполняем Subcategories для каждого родителя
        foreach (var category in items)
            if (categoriesByParent.TryGetValue(category.Id, out var subcategories))
                foreach (var subcategory in subcategories) {
                    subcategory.ParentCategory = category;
                    category.AddSubcategory(subcategory);
                }
        
        var rootCategories = items.Where(c => !c.ParentCategoryId.HasValue);
        return rootCategories.ToList();
    }
}
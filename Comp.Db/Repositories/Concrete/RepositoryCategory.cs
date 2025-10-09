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


    public override Task AddAsync(Category entity) {
        if (entity.ParentCategory != null && WouldCreateCycle(entity, entity.ParentCategory))
            throw new InvalidOperationException();
        return base.AddAsync(entity);
    }

    public override Task UpdateAsync(Category entity) {
        if (entity.ParentCategory != null && WouldCreateCycle(entity, entity.ParentCategory))
            throw new InvalidOperationException();
        return base.UpdateAsync(entity);
    }


    /// <summary>
    /// Проверяет, не создаст ли добавление родительской категории циклическую ссылку
    /// </summary>
    /// <param name="category">Категория, для которой проверяется родитель</param>
    /// <param name="potentialParent">Потенциальный родитель</param>
    /// <returns>True, если добавление создаст цикл, False в противном случае</returns>
    public static bool WouldCreateCycle(Category category, Category potentialParent) {
        // Текущая категория не может быть собственным родителем
        if (category.Id == potentialParent.Id)
            return true;

        // Проверяем цепочку родителей потенциального родителя
        var currentParent = potentialParent;
        var visitedIds = new HashSet<int> { category.Id };

        while (currentParent != null) {
            // Если в цепочке родителей встречается текущая категория, это цикл
            if (visitedIds.Contains(currentParent.Id))
                return true;

            visitedIds.Add(currentParent.Id);

            // Переходим к следующему родителю
            currentParent = currentParent.ParentCategory;
        }
        return false;
    }
}
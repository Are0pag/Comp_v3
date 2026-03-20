using Comp.ModelData.SortingItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class RepositoryCategory : DbRepository<Category>
{
    public RepositoryCategory(AppDbContext context) : base(context) {
    }

    public override async Task<List<Category>> GetAllAsync() {
        var categories = await _context.Set<Category>()
                                       .AsNoTracking()
                                       .Include(c => c.ParentCategory)
                                       .ToListAsync();

        var categoriesByParent = categories
                                .Where(c => c.ParentCategoryId.HasValue)
                                .GroupBy(c => c.ParentCategoryId!.Value)
                                .ToDictionary(g => g.Key, g => g.ToList());
        
        // Заполняем Subcategories для каждого родителя
        foreach (var category in categories)
            if (categoriesByParent.TryGetValue(category.Id, out var subcategories))
                foreach (var subcategory in subcategories) {
                    //subcategory.ParentCategory = category;
                    category.AddSubcategory(subcategory);
                }
        
        var rootCategories = categories.Where(c => !c.ParentCategoryId.HasValue);
        return rootCategories.ToList();
    }


    public override Task AddAsync(Category entity) {
        ArgumentNullException.ThrowIfNull(entity.Name);
        
        entity.Id = default;
        if (entity.ParentCategory != null) {
            entity.ParentCategory = _context.Set<Category>()
                                            .FirstOrDefault(c => c.Id == entity.ParentCategory.Id)
                                    ?? throw new KeyNotFoundException();
        }
        
        if (entity.ParentCategory != null && WouldCreateCycle(category: entity, potentialParent: entity.ParentCategory))
            throw new InvalidOperationException();
        return base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Category entity) {
        ArgumentNullException.ThrowIfNull(entity.Name);
        entity.Id.ThrowIfDefault();
        
        if (entity.ParentCategory != null && WouldCreateCycle(entity, entity.ParentCategory))
            throw new InvalidOperationException();

        var trackedEntity = _context.Set<Category>()
                                    .Include(category => category.ParentCategory)
                                    .FirstOrDefault(c => c.Id == entity.Id)
                            ?? throw new KeyNotFoundException();
        
        trackedEntity.PopulateFrom(entity);

        if (trackedEntity.ParentCategory != null && trackedEntity.ParentCategory.Id != trackedEntity.ParentCategoryId)
            trackedEntity.ParentCategory = _context.Set<Category>()
                                                   .FirstOrDefault(c => c.Id == trackedEntity.ParentCategoryId)
                                           ?? throw new KeyNotFoundException();
        
        _context.Entry(trackedEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(int id) {
        /*var trackedEntity = _context.Set<Category>()
                             .FirstOrDefault(c => c.Id == id)
                     ?? throw new KeyNotFoundException();   

        foreach (var category in await GetAllAsync()) {
            
        }*/
        
        var allCategories = await _context.Set<Category>()
                                          .Include(c => c.ParentCategory)
                                          .ToListAsync();
    
        // 2. Находим категорию для удаления (уже отслеживаемую)
        var categoryToDelete = allCategories.FirstOrDefault(c => c.Id == id);
        if (categoryToDelete == null) return;
    
        // 3. Рекурсивно находим ВСЕ дочерние категории
        var allChildren = GetAllChildrenRecursive(categoryToDelete, allCategories);
    
        // 4. Удаляем все найденные категории
        foreach (var child in allChildren) {
            _context.Set<Category>().Remove(child);
        }
    
        // 5. Удаляем саму категорию
        _context.Set<Category>().Remove(categoryToDelete);
    
        await _context.SaveChangesAsync();
    }
    
    private List<Category> GetAllChildrenRecursive(Category parent, List<Category> allCategories) {
        var result = new List<Category>();
    
        // Находим прямых потомков
        var directChildren = allCategories.Where(c => c.ParentCategoryId == parent.Id).ToList();
    
        foreach (var child in directChildren) {
            result.Add(child);
            result.AddRange(GetAllChildrenRecursive(child, allCategories));
        }
    
        return result;
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
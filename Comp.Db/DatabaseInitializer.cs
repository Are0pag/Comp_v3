using Comp.ModelData.SortingItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db;

public class DatabaseInitializer
{
    private readonly AppDbContext _context;
    public DatabaseInitializer(AppDbContext context) {
        _context = context;
    }

    public const string ROOT_CATEGORY_NAME = "Компоненты";

    public async Task InitializeAsync() {
        await _context.Database.EnsureCreatedAsync();
        await EnsureRootCategoryExistsAsync();
    }

    private async Task EnsureRootCategoryExistsAsync() {
        // Проверяем, есть ли корневая категория
        var rootCategoryExists = await _context.Categories
                                               .AnyAsync(c => c.ParentCategoryId == null);

        if (!rootCategoryExists) {
            var rootCategory = new Category(ROOT_CATEGORY_NAME, null);
            _context.Categories.Add(rootCategory);
            await _context.SaveChangesAsync();
        }
    }

    /*private async Task EnsureDefaultGenericParametersSetExistsAsync() {
        var defaultValue = _context.GenericParametersSets
                                   .AddAsync(g => g.)
    }*/
}
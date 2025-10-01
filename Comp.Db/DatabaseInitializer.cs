using Comp.ModelData.SortingItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db;

public class DatabaseInitializer
{
    private readonly AppDbContext _context;
    public DatabaseInitializer(AppDbContext context) {
        _context = context;
    }

    public async Task InitializeAsync() {
        await _context.Database.EnsureCreatedAsync();
        await EnsureRootCategoryExistsAsync();
    }

    private async Task EnsureRootCategoryExistsAsync() {
        // Проверяем, есть ли корневая категория
        var rootCategoryExists = await _context.Categories
                                               .AnyAsync(c => c.ParentCategoryId == null);

        if (!rootCategoryExists) {
            var rootCategory = new Category("Компоненты", null);
            _context.Categories.Add(rootCategory);
            await _context.SaveChangesAsync();
        }
    }
}
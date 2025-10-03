using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;

namespace Comp.Db.Repositories.Concrete;

public class RepositoryComponent : DbRepository<Component>
{
    protected readonly IRepository<Category> _categoryRepository;
    
    public RepositoryComponent(AppDbContext context, IRepository<Category> categoryRepository) : base(context) {
        _categoryRepository = categoryRepository;
    }

    public override async Task<List<Component>> GetAllAsync() {
        var copms = await base.GetAllAsync();
        foreach (var copm in copms) {
            copm.Category = await _categoryRepository.GetByIdAsync(copm.CategoryId);
        }
        return copms;
    }

    public override async Task AddAsync(Component entity) {
        entity.Category = await _categoryRepository.GetByIdAsync(entity.Category.Id);
        await base.AddAsync(entity);
    }
}
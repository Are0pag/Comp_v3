using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;

namespace Comp.Db.Repositories.Concrete;

public class RepositoryComponent : DbRepository<Component>
{
    protected readonly IRepository<Category> _categoryRepository;
    protected readonly IRepository<ConditionalDesignation> _conditionalDesignationRepository;
    
    public RepositoryComponent(AppDbContext context, IRepository<Category> categoryRepository, IRepository<ConditionalDesignation> conditionalDesignationRepository) : base(context) {
        _categoryRepository = categoryRepository;
        _conditionalDesignationRepository = conditionalDesignationRepository;
    }

    public override async Task<List<Component>> GetAllAsync() {
        var copms = await base.GetAllAsync();
        foreach (var copm in copms) {
            copm.Category = await _categoryRepository.GetByIdAsync(copm.CategoryId);

            if (copm.ConditionalDesignationId != null) {
                int id = copm.ConditionalDesignationId.Value;
                copm.ConditionalDesignation = await _conditionalDesignationRepository.GetByIdAsync(id);
            }
            
        }
        return copms;
    }

    public override async Task AddAsync(Component entity) {
        entity.Category = await _categoryRepository.GetByIdAsync(entity.Category.Id);
        entity.Id = default;
        entity.CategoryId = default;
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Component entity) {
        try {
            if (await GetByIdAsync(entity.Id) is not {} dbInstance)
                throw new NullReferenceException("Db Instance could not be found");
            
            dbInstance.ConditionalDesignationId = entity.ConditionalDesignation?.Id ?? null;
            
            _context.Set<Component>().Update(dbInstance);
            await _context.SaveChangesAsync();
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }
}